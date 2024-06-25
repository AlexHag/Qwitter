using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.BankAccount.Models;

namespace Qwitter.Ledger.BankAccount.Repositories;

public interface ISystemBankAccountRepository
{
    Task<SystemBankAccountEntity?> GetByCurrency(string currency);
    Task Update(SystemBankAccountEntity account);
}

public class SystemBankAccountRepository : ISystemBankAccountRepository
{
    private readonly AppDbContext _dbContext;

    public SystemBankAccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SystemBankAccountEntity?> GetByCurrency(string currency)
    {
        return await _dbContext.SystemBankAccounts.FirstOrDefaultAsync(x => x.Currency == currency);
    }

    public async Task Update(SystemBankAccountEntity account)
    {
        _dbContext.SystemBankAccounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }
}