using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Contract.CryptoTransfer;
using Qwitter.Crypto.Contract.CryptoTransfer.Events;
using Qwitter.Crypto.Contract.CryptoTransfer.Models;
using Qwitter.Crypto.Service.Configuration;
using Qwitter.Crypto.Service.CryptoTransfer.Models;
using Qwitter.Crypto.Service.CryptoTransfer.Repositories;
using Qwitter.Crypto.Service.Wallet.Repositories;

namespace Qwitter.Crypto.Service.CryptoTransfer;

[ApiController]
[Route("crypto-transfer")]
public class CryptoTransferService : ControllerBase, ICryptoTransferService
{
    private readonly ICryptoTransferRepository _cryptoTransferRepository;
    private readonly ICryptoOutWalletRepository _cryptoOutWalletRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IEventProducer _eventProducer;
    private readonly CryptoConfig _cryptoConfig;
    private readonly IMapper _mapper;

    public CryptoTransferService(
        ICryptoTransferRepository cryptoTransferRepository,
        ICryptoOutWalletRepository cryptoOutWalletRepository,
        IWalletRepository walletRepository,
        IEventProducer eventProducer,
        CryptoConfig cryptoConfig,
        IMapper mapper)
    {
        _cryptoTransferRepository = cryptoTransferRepository;
        _cryptoOutWalletRepository = cryptoOutWalletRepository;
        _walletRepository = walletRepository;
        _eventProducer = eventProducer;
        _cryptoConfig = cryptoConfig;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<CreateCryptoTransferResponse> CreateCryptoTransfer(CreateCryptoTransferRequest request)
    {
        if (!_cryptoConfig.SupportedCurrencies.Contains(request.Currency))
        {
            throw new ArgumentException("Currency is not supported");
        }

        var cryptoOutWallet = await _cryptoOutWalletRepository.GetWalletByCrurrency(request.Currency);
        var wallet = await _walletRepository.GetById(cryptoOutWallet.WalletId);

        if (wallet.Balance < request.Amount)
        {
            throw new ArgumentException("Insufficient funds");
        }

        var transaction = new CryptoTransferEntity
        {
            TransactionId = Guid.NewGuid(),
            SourceAddress = wallet.Address,
            DestinationAddress = request.Address,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = CryptoTransferStatus.Pending,
            SubTopic = request.SubTopic
        };

        await _cryptoTransferRepository.Insert(transaction);

        await _eventProducer.Produce(new ProcessCryptoTransferEvent { TransactionId = transaction.TransactionId });

        return new CreateCryptoTransferResponse { TransactionId = transaction.TransactionId };
    }

    [HttpGet("{transactionId}")]
    public async Task<CryptoTransferResponse> GetTransfer(Guid transactionId)
    {
        var transaction = await _cryptoTransferRepository.GetById(transactionId);
        return _mapper.Map<CryptoTransferResponse>(transaction);
    }

    [HttpGet("process/{transactionId}")]
    public async Task Process(Guid transactionId)
    {
        await _eventProducer.Produce(new ProcessCryptoTransferEvent { TransactionId = transactionId });
    }
}