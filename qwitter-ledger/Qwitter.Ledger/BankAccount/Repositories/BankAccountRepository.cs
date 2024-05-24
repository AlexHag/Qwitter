using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.BankAccount.Models;

namespace Qwitter.Ledger.BankAccount.Repositories;

public interface IBankAccountRepository
{
    Task Insert(BankAccountEntity bankAccount);
    Task<BankAccountEntity?> GetById(Guid id);
    Task<BankAccountEntity?> GetByAccountNumber(string accountNumber);
    Task<List<BankAccountEntity>> GetAllByUserId(Guid userId);
    Task Update(BankAccountEntity account);
}

public class BankAccountRepository : IBankAccountRepository
{
    private readonly AppDbContext _dbContext;

    public BankAccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(BankAccountEntity bankAccount)
    {
        await _dbContext.BankAccounts.AddAsync(bankAccount);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BankAccountEntity?> GetById(Guid id)
    {
        return await _dbContext.BankAccounts.FindAsync(id);
    }

    public async Task Update(BankAccountEntity bankAccount)
    {
        bankAccount.UpdatedAt = DateTime.UtcNow;
        _dbContext.BankAccounts.Update(bankAccount);
        await _dbContext.SaveChangesAsync();
    }

    public Task<BankAccountEntity?> GetByAccountNumber(string accountNumber)
    {
        return _dbContext.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public Task<List<BankAccountEntity>> GetAllByUserId(Guid userId)
    {
        return _dbContext.BankAccounts.Where(a => a.UserId == userId).ToListAsync();
    }
}