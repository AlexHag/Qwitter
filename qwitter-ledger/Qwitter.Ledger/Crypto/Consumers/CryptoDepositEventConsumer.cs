
using MassTransit;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Contract.Wallets.Events;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.Crypto.Repositories;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.Transactions.Services;

namespace Qwitter.Ledger.Crypto.Consumers;

[MessageSuffixAttribute(App.Name)]
public class CryptoDepositEventConsumer : IConsumer<CryptoDepositEvent>
{
    private readonly ILogger<CryptoDepositEventConsumer> _logger;
    private readonly IBankAccountCryptoWalletRepository _bankAccountCryptoWalletRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionService _transactionService;

    public CryptoDepositEventConsumer(
        ILogger<CryptoDepositEventConsumer> logger,
        IBankAccountCryptoWalletRepository bankAccountCryptoWalletRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionService transactionService)
    {
        _logger = logger;
        _bankAccountCryptoWalletRepository = bankAccountCryptoWalletRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionService = transactionService;
    }

    public async Task Consume(ConsumeContext<CryptoDepositEvent> context)
    {
        // TODO: Credit the system
        _logger.LogInformation("Consuming CryptoDepositEvent {WalletId} {TransactionHash} {Amount} {Currency}", context.Message.WalletId, context.Message.TransactionHash, context.Message.Amount, context.Message.Currency);
        var wallet = await _bankAccountCryptoWalletRepository.GetByWalletId(context.Message.WalletId);

        // TODO: Handle exceptions
        if (wallet is null)
        {
            _logger.LogError("Wallet not found {WalletId}", context.Message.WalletId);
            return;
        }

        // var creditFundsRequest = new CreditFundsRequest
        // {
        //     UserId = wallet.UserId,
        //     BankAccountId = wallet.BankAccountId,
        //     Amount = context.Message.Amount,
        //     Currency = context.Message.Currency,
        //     Message = $"Crypto deposit. Transaction hash: {context.Message.TransactionHash}"
        // };

        // TODO: Handle blocked or unverified accounts
        // Credit the account
        // await _transactionService.CreditFunds(creditFundsRequest);
    }
}