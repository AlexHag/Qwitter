
using MassTransit;
using Qwitter.Payments.Contract.Transactions.Events;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Provider;
using Qwitter.Payments.Transactions.Configuration;
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
    private readonly TransactionConfiguration _transactionConfig;

    public TransactionCompletedConsumer(
        ILogger<TransactionCompletedConsumer> logger,
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        IPaymentProviderService paymentProvider,
        TransactionConfiguration transactionConfig)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _paymentProvider = paymentProvider;
        _transactionConfig = transactionConfig;
    }

    public async Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        if (_transactionConfig.TransferFundsAfterCompletion)
        {
            await TransferToRootWallet(context.Message);
        }
    }

    private async Task TransferToRootWallet(TransactionCompletedEvent context)
    {
        var transaction = await _transactionRepository.GetById(context.TransactionId);
        
        if (transaction is null)
        {
            _logger.LogWarning("Received transaction completed event but transaction was not found. TransactionId: {TransactionId}", context.TransactionId);
            return;
        }

        var wallet = await _walletRepository.GetById(transaction.WalletId);

        if (wallet is null)
        {
            _logger.LogWarning("Received transaction completed event but wallet was not found. TransactionId: {TransactionId}, WalletId: {WalletId}", transaction.Id, transaction.WalletId);
            return;
        }

        var success = await _paymentProvider.Transfer(wallet.PrivateKey, _transactionConfig.WithdrawingAddress, transaction.Currency);

        if (success)
        {
            _logger.LogInformation("Wallet withdrawn successfully. TransactionId: {TransactionId}, WalletId: {WalletId}", transaction.Id, transaction.WalletId);
            // Fix
            transaction.Status = TransactionStatus.Withdrawn;
            await _transactionRepository.Update(transaction);
        }
        else
        {
            _logger.LogWarning("Failed to withdraw funds from wallet. TransactionId: {TransactionId}, WalletId: {WalletId}", transaction.Id, transaction.WalletId);
        }
    }
}