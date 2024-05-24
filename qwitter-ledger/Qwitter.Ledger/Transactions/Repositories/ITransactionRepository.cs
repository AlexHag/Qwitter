using Qwitter.Ledger.Transactions.Models;

namespace Qwitter.Ledger.Transactions.Repositories;

public interface ITransactionRepository
{
    Task Insert(TransactionEntity entity);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(TransactionEntity entity)
    {
        await _dbContext.Transactions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
}