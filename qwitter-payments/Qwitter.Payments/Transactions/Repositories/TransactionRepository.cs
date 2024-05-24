
using Qwitter.Core.Application.Exceptions;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Transactions.Models;

namespace Qwitter.Payments.Transactions.Repositories;

public interface ITransactionRepository
{
    Task Insert(TransactionEntity transaction);
    Task Update(TransactionEntity transaction);
    Task<TransactionEntity?> GetById(Guid transactionId);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TransactionEntity?> GetById(Guid transactionId)
    {
        var entity = await _dbContext.Transactions.FindAsync(transactionId);
        return entity;
    }

    public async Task Insert(TransactionEntity transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(TransactionEntity transaction)
    {
        transaction.UpdatedAt = DateTime.UtcNow;
        _dbContext.Transactions.Update(transaction);
        await _dbContext.SaveChangesAsync();
    }
}