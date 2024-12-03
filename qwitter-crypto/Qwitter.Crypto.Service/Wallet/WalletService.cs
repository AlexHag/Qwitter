using MapsterMapper;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Contract.Wallet.Events;
using Qwitter.Crypto.Contract.Wallet.Models;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Service.Wallet.Models;
using Qwitter.Crypto.Service.Wallet.Repositories;
using Qwitter.Crypto.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Crypto.Service.CryptoTransfer.Repositories;
using Qwitter.Crypto.Service.CryptoTransfer.Models;
using Qwitter.Crypto.Contract.CryptoTransfer.Models;
using Qwitter.Crypto.Currency.Contract.Models;

namespace Qwitter.Crypto.Service.Wallet;

[ApiController]
[Route("wallet")]
public class WalletService : ControllerBase, IWalletService
{
    private readonly IMapper _mapper;
    private readonly ILogger<WalletService> _logger;
    private readonly IWalletRepository _walletRepository;
    private readonly ICryptoTransferRepository _cryptoTransferRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventProducer _eventProducer;

    public WalletService(
        IMapper mapper,
        ILogger<WalletService> logger,
        IWalletRepository walletRepository,
        ICryptoTransferRepository cryptoTransferRepository,
        IServiceProvider serviceProvider,
        IEventProducer eventProducer)
    {
        _mapper = mapper;
        _logger = logger;
        _walletRepository = walletRepository;
        _cryptoTransferRepository = cryptoTransferRepository;
        _serviceProvider = serviceProvider;
        _eventProducer = eventProducer;
    }

    [HttpPost]
    public async Task<WalletResponse> CreateWallet(CreateWalletRequest request)
    {
        // Fix: This throws InvalidOperationException if the currency is not supported
        var walletService = _serviceProvider.GetRequiredKeyedService<ICryptoWalletService>(request.Currency) ?? throw new NotFoundApiException($"{request.Currency} is not supported yet");
        var wallet = await walletService.CreateWallet();

        var entity = new WalletEntity
        {
            Id = Guid.NewGuid(),
            Currency = wallet.Currency,
            Address = wallet.Address,
            Balance = 0,
            PrivateKey = wallet.PrivateKey,
            SubTopic = request.SubTopic
        };

        await _walletRepository.InsertWallet(entity);

        return _mapper.Map<WalletResponse>(entity);
    }

    [HttpGet("id/{walletId}")]
    public async Task<WalletResponse> GetWalletById(Guid walletId)
    {
        var wallet = await _walletRepository.GetById(walletId);
        return _mapper.Map<WalletResponse>(wallet);
    }

    [HttpPut("sync/{address}")]
    public async Task<SyncWalletResponse> SyncWallet(string address)
    {
        var wallet = await _walletRepository.GetByAddress(address) ?? throw new NotFoundApiException("Wallet not found");

        var walletService = _serviceProvider.GetRequiredKeyedService<ICryptoWalletService>(wallet.Currency) ?? throw new NotImplementedException($"{wallet.Currency} is not supported yet");

        var existingTransfers = await _cryptoTransferRepository.GetByDestinationAddress(wallet.Address);

        var newTransfers = new List<CryptoTransferModel>();

        if (!existingTransfers.Any())
        {
            newTransfers = (await walletService.GetWalletTransfers(wallet.Address)).ToList();
        }
        else
        {
            var latestAccountedBlock = existingTransfers.Max(p => p.BlockNumber) + 1;
            newTransfers = (await walletService.GetWalletTransferSinceBlockNumber(wallet.Address, latestAccountedBlock ?? 0)).ToList();
        }

        decimal depositAmount = 0;

        foreach (var transfer in newTransfers)
        {
            if (existingTransfers.Any(p => p.TransactionHash == transfer.TransactionHash))
            {
                _logger.LogWarning("Received duplicate transfer {TransactionHash}", transfer.TransactionHash);
                continue;
            }

            await _eventProducer.Produce(new CryptoDepositEvent
            {
                WalletId = wallet.Id,
                TransactionHash = transfer.TransactionHash,
                Amount = transfer.Amount,
                Currency = wallet.Currency
            }, wallet.SubTopic);

            var entity = _mapper.Map<CryptoTransferEntity>(transfer);
            entity.TransactionId = Guid.NewGuid();

            if (entity.BlockHash != null || entity.BlockNumber != null)
            {
                entity.Status = CryptoTransferStatus.Completed;
            }
            else
            {
                entity.Status = CryptoTransferStatus.Initiated;
            }
        
            await _cryptoTransferRepository.Insert(entity);
            depositAmount += transfer.Amount;
        }

        if (depositAmount > 0)
        {
            wallet.Balance += depositAmount;
            await _walletRepository.Update(wallet);
        }

        return new SyncWalletResponse
        {
            Count = newTransfers.Count,
            DepositAmount = depositAmount
        };
    }
}