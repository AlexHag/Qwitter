using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Transactions.Models;

namespace Qwitter.Ledger.Transactions.Repositories;

public interface ITransactionRepository
{
    Task Insert(TransactionEntity entity);
    Task<IEnumerable<TransactionEntity>> GetByAccountId(Guid accountId, PaginationRequest request);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TransactionEntity>> GetByAccountId(Guid accountId, PaginationRequest request)
    {
        var query = await _dbContext.Transactions
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(request.Offset)
            .Take(request.Take)
            .ToListAsync();

        return query;
    }

    public async Task Insert(TransactionEntity entity)
    {
        await _dbContext.Transactions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
}