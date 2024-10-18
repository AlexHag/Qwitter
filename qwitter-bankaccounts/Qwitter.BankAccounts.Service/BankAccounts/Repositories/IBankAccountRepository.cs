
using Microsoft.EntityFrameworkCore;
using Qwitter.BankAccounts.Service.BankAccounts.Models;

namespace Qwitter.BankAccounts.Service.BankAccounts.Repositorie;

public interface IBankAccountRepository
{
    Task Insert(BankAccountEntity entity);
    Task Update(BankAccountEntity entity);
    Task<BankAccountEntity> GetById(Guid id);
    Task<BankAccountEntity> GetByAccountNumber(string accountNumber);
    Task<List<BankAccountEntity>> GetAllByUserId(Guid userId);
    Task<BankAccountEntity?> TryGetDefaultByUserId(Guid userId);
}

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ServiceDbContext _dbContext;

    public BankAccountRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(BankAccountEntity entity)
    {
        await _dbContext.BankAccounts.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(BankAccountEntity entity)
    {
        _dbContext.BankAccounts.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BankAccountEntity> GetByAccountNumber(string accountNumber)
    {
        var entity = await _dbContext.BankAccounts.FirstOrDefaultAsync(p => p.AccountNumber == accountNumber);
        
        if (entity == null)
        {
            throw new Exception($"Bank account with account number: {accountNumber} not found");
        }

        return entity;
    }

    public async Task<List<BankAccountEntity>> GetAllByUserId(Guid userId)
        => await _dbContext.BankAccounts.Where(p => p.UserId == userId).ToListAsync();

    public async Task<BankAccountEntity> GetById(Guid id)
    {
        var entity = await _dbContext.BankAccounts.FirstOrDefaultAsync(p => p.BankAccountId == id);
        
        if (entity == null)
        {
            throw new Exception($"Bank account with id: {id} not found");
        }

        return entity;
    }

    public Task<BankAccountEntity?> TryGetDefaultByUserId(Guid userId)
    {
        return _dbContext.BankAccounts.FirstOrDefaultAsync(p => p.UserId == userId && p.IsDefault);
    }
}