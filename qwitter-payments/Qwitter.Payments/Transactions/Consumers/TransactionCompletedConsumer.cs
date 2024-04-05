
using MassTransit;
using Qwitter.Payments.Contract.Transactions.Events;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Provider;
using Qwitter.Payments.Transactions.Models;
using Qwitter.Payments.Transactions.Repositories;
using Qwitter.Payments.Wallets.Repositories;

namespace Qwitter.Payments.Transactions.Consumers;

public class TransactionCompletedConsumer : IConsumer<TransactionCompletedEvent>
{
    private readonly ILogger<TransactionCompletedConsumer> _logger;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IPaymentProviderService _paymentProvider;

    // TODO: Configure this address in appsettings or database
    private readonly string _withdrawingAddress = "0xdae90dB462A74F6C0eB8e93B10c596108921ba10";

    public TransactionCompletedConsumer(
        ILogger<TransactionCompletedConsumer> logger,
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        IPaymentProviderService paymentProvider)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _paymentProvider = paymentProvider;
    }

    public async Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        var transaction = await _transactionRepository.GetTransactionById(context.Message.TransactionId);
        
        if (transaction is null)
        {
            _logger.LogWarning("Received transaction completed event but transaction was not found. TransactionId: {TransactionId}", context.Message.TransactionId);
            return;
        }

        var wallet = await _walletRepository.GetWalletById(transaction.WalletId);

        if (wallet is null)
        {
            _logger.LogWarning("Received transaction completed event but wallet was not found. TransactionId: {TransactionId}, WalletId: {WalletId}", transaction.Id, transaction.WalletId);
            return;
        }

        var success = await _paymentProvider.Transfer(wallet.PrivateKey, _withdrawingAddress);

        if (success)
        {
            _logger.LogInformation("Wallet withdrawn successfully. TransactionId: {TransactionId}, WalletId: {WalletId}", transaction.Id, transaction.WalletId);
            await _transactionRepository.UpdateTransaction(new TransactionUpdateModel
            {
                Id = transaction.Id,
                Status = TransactionStatus.Withdrawn
            });
        }
        else
        {
            _logger.LogWarning("Failed to withdraw funds from wallet. TransactionId: {TransactionId}, WalletId: {WalletId}", transaction.Id, transaction.WalletId);
        }
    }
}