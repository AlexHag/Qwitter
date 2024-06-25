using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.Transactions.Models;

namespace Qwitter.Ledger.Transactions.Repositories;

public interface IBankAccountTransactionRepository
{
    Task Insert(BankAccountTransactionEntity entity);
    Task<BankAccountTransactionEntity?> GetById(Guid id);
    Task Update(BankAccountTransactionEntity entity);
}

public class BankAccountTransactionRepository : IBankAccountTransactionRepository
{
    private readonly AppDbContext _dbContext;

    public BankAccountTransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(BankAccountTransactionEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbContext.AccountTransactions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BankAccountTransactionEntity?> GetById(Guid id)
        => await _dbContext.AccountTransactions.FirstOrDefaultAsync(x => x.Id == id);

    public async Task Update(BankAccountTransactionEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbContext.AccountTransactions.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}
