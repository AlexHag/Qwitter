using Microsoft.EntityFrameworkCore;
using Qwitter.SystemLedger.Models;

namespace Qwitter.Ledger.SystemLedger.Repositories;

public interface ISystemAccountRepository
{
    Task<SystemAccountEntity?> GetByCurrency(string currency);
    Task Update(SystemAccountEntity account);
}

public class SystemAccountRepository : ISystemAccountRepository
{
    private readonly AppDbContext _dbContext;

    public SystemAccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SystemAccountEntity?> GetByCurrency(string currency)
    {
        return await _dbContext.SystemAccounts.FirstOrDefaultAsync(x => x.Currency == currency);
    }

    public async Task Update(SystemAccountEntity account)
    {
        _dbContext.SystemAccounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }
}