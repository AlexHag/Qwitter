using Qwitter.Ledger.BankAccount.Repositories;

namespace Qwitter.Ledger.Transactions.Services;

public interface ISystemTransactionService
{
    // TODO: Make parameters a command and include references
    Task DebitSystemCurrency(string currency, decimal amount);
    Task CreditSystemCurrency(string currency, decimal amount);
}

public class SystemTransactionService : ISystemTransactionService
{
    private readonly ISystemBankAccountRepository _repository;

    public SystemTransactionService(ISystemBankAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task CreditSystemCurrency(string currency, decimal amount)
    {
        var account = await _repository.GetByCurrency(currency);

        if (account == null)
        {
            throw new Exception("System account not found");
        }

        // TODO: Create system transaction table

        account.Balance += amount;
        await _repository.Update(account);
    }

    public async Task DebitSystemCurrency(string currency, decimal amount)
    {
        var account = await _repository.GetByCurrency(currency);

        if (account == null)
        {
            throw new Exception("System account not found");
        }

        // TODO: Implement low system balance monitor

        account.Balance -= amount;
        await _repository.Update(account);
    }
}