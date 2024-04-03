using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
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

    public TransactionService(
        ITransactionRepository transactionRepository,
        IWalletService walletService,
        IEventProducer producer,
        IPaymentProviderService paymentProvider)
    {
        _transactionRepository = transactionRepository;
        _walletService = walletService;
        _eventProducer = producer;
        _paymentProvider = paymentProvider;
    }

    public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request)
    {
        var wallet = await _walletService.CreateWallet(request.UserId);

        var transaction = await _transactionRepository.InsertTransaction(new TransactionInsertModel
        {
            UserId = request.UserId,
            WalletId = wallet.Id,
            PaymentAddress = wallet.Address,
            Topic = request.Topic,
            Amount = request.Amount
        });

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
            Topic = request.Topic,
            PaymentAddress = wallet.Address,
        };
    }

    public async Task SyncTransaction(Guid transactionId)
    {
        var transaction = await _transactionRepository.GetTransactionById(transactionId);
        
        if (transaction is null)
            throw new NotFoundApiException("Transaction not found");
        
        if (transaction.Status != TransactionStatus.Pending)
            return;
        
        var amountReceived = await _paymentProvider.GetAmountReceived(transaction.PaymentAddress);

        if (amountReceived >= transaction.Amount)
        {
            await _transactionRepository.UpdateTransaction(new TransactionUpdateModel
            {
                Id = transaction.Id,
                AmountReceived = amountReceived,
                Status = TransactionStatus.Completed
            });

            await _eventProducer.Produce(new TransactionCompletedEvent
            {
                UserId = transaction.UserId,
                TransactionId = transaction.Id
            }, transaction.Topic);
        }
        else if (amountReceived != transaction.AmountReceived)
        {
            await _transactionRepository.UpdateTransaction(new TransactionUpdateModel
            {
                Id = transaction.Id,
                AmountReceived = amountReceived
            });
        }
    }
}
