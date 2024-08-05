using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.SystemLedger.Models;
using Qwitter.Ledger.SystemLedger.Repositories;

namespace Qwitter.Ledger.SystemLedger.Services;

public interface ISystemTransactionService
{
    // TODO: Make parameters a command and include references
    Task DebitSystemCurrency(Guid allocationId, string currency, decimal amount);
    Task CreditSystemCurrency(Guid allocationId, string currency, decimal amount);
}

public class SystemTransactionService : ISystemTransactionService
{
    private readonly ISystemAccountRepository _accountRepository;
    private readonly ISystemTransactionRepository _transactionRepository;

    public SystemTransactionService(
        ISystemAccountRepository accountRepository,
        ISystemTransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task CreditSystemCurrency(Guid allocationId, string currency, decimal amount)
    {
        var account = await _accountRepository.GetByCurrency(currency);

        if (account == null)
        {
            throw new Exception("System account not found");
        }

        var transaction = new SystemTransactionEntity
        {
            Id = Guid.NewGuid(),
            FundAllocationId = allocationId,
            Amount = amount,
            Currency = currency,
            Type = TransactionType.Credit
        };

        await _transactionRepository.Insert(transaction);

        account.Balance += amount;
        await _accountRepository.Update(account);
    }

    public async Task DebitSystemCurrency(Guid allocationId, string currency, decimal amount)
    {
        var account = await _accountRepository.GetByCurrency(currency);

        if (account == null)
        {
            throw new Exception("System account not found");
        }

        var transaction = new SystemTransactionEntity
        {
            Id = Guid.NewGuid(),
            FundAllocationId = allocationId,
            Amount = amount,
            Currency = currency,
            Type = TransactionType.Debit
        };

        await _transactionRepository.Insert(transaction);

        account.Balance -= amount;
        await _accountRepository.Update(account);
    }
}