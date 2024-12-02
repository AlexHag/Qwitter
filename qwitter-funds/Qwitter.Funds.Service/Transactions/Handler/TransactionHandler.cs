using Qwitter.Funds.Contract.Transactions.Enums;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Transactions.Models;
using Qwitter.Funds.Service.Transactions.Repositories;

namespace Qwitter.Funds.Service.Transactions.Handler;

public interface ITransactionHandler
{
    Task CreditFunds(TransactionCommand command);
    Task DebitFunds(TransactionCommand command);
}

public class TransactionHandler : ITransactionHandler
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionHandler(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task CreditFunds(TransactionCommand command)
        => await Execute(command, TransactionType.Credit);

    public async Task DebitFunds(TransactionCommand command)
        => await Execute(command, TransactionType.Debit);

    private async Task Execute(TransactionCommand command, TransactionType type)
    {
        var duplicateTransaction = await _transactionRepository.TryGetByAllocationId(command.AllocationId, type);

        if (duplicateTransaction != null)
        {
            return;
        }

        var account = await _accountRepository.GetById(command.AccountId);

        if (account.Currency != command.Currency)
        {
            throw new InvalidOperationException("Currency mismatch");
        }

        var previousBalance = account.Balance;

        account.Balance = type == TransactionType.Credit
            ? account.Balance + command.Amount
            : account.Balance - command.Amount;

        var transaction = new TransactionEntity
        {
            TransactionId = Guid.NewGuid(),
            AccountId = account.AccountId,
            AllocationId = command.AllocationId,
            Currency = command.Currency,
            Amount = command.Amount,
            PreviousBalance = previousBalance,
            NewBalance = account.Balance,
            Type = type
        };

        await _transactionRepository.Insert(transaction);
        await _accountRepository.Update(account);
    }
}