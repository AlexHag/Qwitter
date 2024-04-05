
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
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IPaymentProviderService _paymentProvider;

    // TODO: Configure this address in appsettings or database
    private readonly string _withdrawingAddress = "0xdae90dB462A74F6C0eB8e93B10c596108921ba10";

    public TransactionCompletedConsumer(
        ITransactionRepository transactionRepository,
        IWalletRepository walletRepository,
        IPaymentProviderService paymentProvider)
    {
        _transactionRepository = transactionRepository;
        _walletRepository = walletRepository;
        _paymentProvider = paymentProvider;
    }

    // TODO: Add logging
    public async Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        var transaction = await _transactionRepository.GetTransactionById(context.Message.TransactionId);
        
        if (transaction is null)
        {
            Console.WriteLine("WARNING: Received transaction completed event but transaction was not found");
            return;
        }

        var wallet = await _walletRepository.GetWalletById(transaction.WalletId);

        if (wallet is null)
        {
            Console.WriteLine("WARNING: Received transaction completed event but wallet was not found");
            return;
        }

        var success = await _paymentProvider.Transfer(wallet.PrivateKey, _withdrawingAddress);

        if (success)
        {
            Console.WriteLine("Wallet withdrawn successfully...");

            await _transactionRepository.UpdateTransaction(new TransactionUpdateModel
            {
                Id = transaction.Id,
                Status = TransactionStatus.Withdrawn
            });
        }
        else
        {
            Console.WriteLine("WARNING: Failed to withdraw funds from wallet");
        }
    }
}