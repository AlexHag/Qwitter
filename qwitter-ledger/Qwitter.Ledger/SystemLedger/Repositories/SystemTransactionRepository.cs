
using Qwitter.Ledger.SystemLedger.Models;

namespace Qwitter.Ledger.SystemLedger.Repositories;

public interface ISystemTransactionRepository
{
    Task Insert(SystemTransactionEntity entity);
}

public class SystemTransactionRepository : ISystemTransactionRepository
{
    private readonly AppDbContext _dbContext;

    public SystemTransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(SystemTransactionEntity entity)
    {
        _dbContext.SystemTransactions.Add(entity);
        await _dbContext.SaveChangesAsync();
    }
}