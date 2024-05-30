
using MapsterMapper;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Contract.Wallets.Events;
using Qwitter.Crypto.Contract.Wallets.Models;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Currency.Contract.Wallets.Models;
using Qwitter.Crypto.Wallets.Models;
using Qwitter.Crypto.Wallets.Repositories;

namespace Qwitter.Crypto.Wallets.Services;

public interface IWalletService
{
    Task<SyncWalletResponse> SyncWallet(string address);
    Task<WalletResponse> CreateWallet(CreateWalletRequest request);
    Task<WalletResponse> GetWalletById(Guid walletId);
}

public class WalletService : IWalletService
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

    public async Task<WalletResponse> GetWalletById(Guid walletId)
    {
        var wallet = await _walletRepository.GetById(walletId) ?? throw new NotFoundApiException("Wallet not found");
        return _mapper.Map<WalletResponse>(wallet);
    }

    public async Task<SyncWalletResponse> SyncWallet(string address)
    {
        var wallet = await _walletRepository.GetByAddress(address) ?? throw new NotFoundApiException("Wallet not found");

        var walletService = _serviceProvider.GetRequiredKeyedService<ICryptoWalletService>(wallet.Currency) ?? throw new NotImplementedException($"{wallet.Currency} is not supported yet");

        var existingTransfers = await _cryptoTransferRepository.GetByToAddress(wallet.Address);

        var newTransfers = new List<CryptoTransfer>();

        if (!existingTransfers.Any())
        {
            newTransfers = (await walletService.GetWalletTransfers(wallet.Address)).ToList();
        }
        else
        {
            var latestAccountedBlock = existingTransfers.Max(p => p.BlockNumber) + 1;
            newTransfers = (await walletService.GetWalletTransferSinceBlockNumber(wallet.Address, latestAccountedBlock)).ToList();
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
            entity.Id = Guid.NewGuid();

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