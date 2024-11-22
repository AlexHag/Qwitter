using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Transactions.Enums;
using Qwitter.Funds.Service.Transactions.Models;

namespace Qwitter.Funds.Service.Transactions.Repositories;

public interface ITransactionRepository
{
    Task Insert(TransactionEntity entity);
    Task<TransactionEntity> GetById(Guid transactionId);
    Task<TransactionEntity?> TryGetByAllocationId(Guid allocationId, TransactionType type);
    Task<List<TransactionEntity>> GetByAccountId(Guid accountId, int skip, int take);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly ServiceDbContext _dbContext;

    public TransactionRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(TransactionEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbContext.Transactions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TransactionEntity> GetById(Guid transactionId)
    {
        var entity = await _dbContext.Transactions.FindAsync(transactionId);

        if (entity == null)
        {
            throw new NotFoundApiException($"Transaction {transactionId} not found");
        }

        return entity;
    }

    public async Task<TransactionEntity?> TryGetByAllocationId(Guid allocationId, TransactionType type)
        => await _dbContext.Transactions.FirstOrDefaultAsync(t => t.AllocationId == allocationId && t.Type == type);

    public Task<List<TransactionEntity>> GetByAccountId(Guid accountId, int skip, int take)
        => _dbContext.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
}