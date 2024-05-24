using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.Account.Models;

namespace Qwitter.Ledger.Account.Repositories;

public interface IAccountRepository
{
    Task Insert(AccountEntity account);
    Task<AccountEntity?> GetById(Guid accountId);
    Task<AccountEntity?> GetByAccountNumber(string accountNumber);
    Task<List<AccountEntity>> GetByUserId(Guid userId);
    Task Update(AccountEntity account);
}

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _dbContext;

    public AccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(AccountEntity account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AccountEntity?> GetById(Guid accountId)
    {
        return await _dbContext.Accounts.FindAsync(accountId);
    }

    public async Task Update(AccountEntity account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }

    public Task<AccountEntity?> GetByAccountNumber(string accountNumber)
    {
        return _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public Task<List<AccountEntity>> GetByUserId(Guid userId)
    {
        return _dbContext.Accounts.Where(a => a.UserId == userId).ToListAsync();
    }
}