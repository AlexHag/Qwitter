using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Payments.Contract.Transactions.Events;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Provider;
using Qwitter.Payments.Transactions.Models;
using Qwitter.Payments.Transactions.Repositories;
using Qwitter.Payments.Wallets.Services;

namespace Qwitter.Payments.Transactions.Services;

public interface ITransactionService
{
    Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request);
    Task SyncTransaction(Guid transactionId);
}

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletService _walletService;
    private readonly IEventProducer _eventProducer;
    private readonly IPaymentProviderService _paymentProvider;
    private readonly PaymentsConfiguration _config;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IWalletService walletService,
        IEventProducer producer,
        IPaymentProviderService paymentProvider,
        PaymentsConfiguration config)
    {
        _transactionRepository = transactionRepository;
        _walletService = walletService;
        _eventProducer = producer;
        _paymentProvider = paymentProvider;
        _config = config;
    }

    public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request)
    {
        if (!_config.SupportedCurrencies.Contains(request.Currency))
        {
            throw new NotSupportedException("Currency not supported");
        }

        var wallet = await _walletService.CreateWallet(request.UserId, request.Currency);

        var transaction = new TransactionEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            WalletId = wallet.Id,
            PaymentAddress = wallet.Address,
            Topic = request.Topic,
            Amount = request.Amount,
            AmountReceived = 0,
            Currency = request.Currency,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _transactionRepository.Insert(transaction);

        await _eventProducer.Produce(new TransactionCreatedEvent
        {
            UserId = request.UserId,
            TransactionId = transaction.Id,
        }, request.Topic);

        return new CreateTransactionResponse
        {
            Id = transaction.Id,
            UserId = request.UserId,
            Amount = request.Amount,
            Currency = request.Currency,
            Topic = request.Topic,
            PaymentAddress = wallet.Address,
        };
    }

    public async Task SyncTransaction(Guid transactionId)
    {
        var transaction = await _transactionRepository.GetById(transactionId);

        if (transaction is null)
            throw new NotFoundApiException("Transaction not found");
        
        if (transaction.Status != TransactionStatus.Pending)
            return;
        
        var amountReceived = await _paymentProvider.GetAmountReceived(transaction.PaymentAddress, transaction.Currency);

        if (amountReceived >= transaction.Amount)
        {
            // Fix
            transaction.AmountReceived = amountReceived;
            transaction.Status = TransactionStatus.Completed;
            await _transactionRepository.Update(transaction);

            await _eventProducer.Produce(new TransactionCompletedEvent
            {
                UserId = transaction.UserId,
                TransactionId = transaction.Id,
                Amount = transaction.Amount,
                Currency = transaction.Currency
            }, transaction.Topic);
        }
        else if (amountReceived != transaction.AmountReceived)
        {
            // Fix
            transaction.AmountReceived = amountReceived;
            await _transactionRepository.Update(transaction);
        }
    }
}
