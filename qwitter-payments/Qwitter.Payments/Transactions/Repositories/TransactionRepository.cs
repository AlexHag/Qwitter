
using Qwitter.Core.Application.Exceptions;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Transactions.Models;

namespace Qwitter.Payments.Transactions.Repositories;

public interface ITransactionRepository
{
    Task<TransactionEntity> InsertTransaction(TransactionInsertModel transaction);
    Task<TransactionEntity> UpdateTransaction(TransactionUpdateModel transaction);
    Task<TransactionEntity?> GetTransactionById(Guid transactionId);
}

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TransactionEntity?> GetTransactionById(Guid transactionId)
    {
        var entity = await _dbContext.Transactions.FindAsync(transactionId);
        return entity;
    }

    public async Task<TransactionEntity> InsertTransaction(TransactionInsertModel transaction)
    {
        var entity = new TransactionEntity
        {
            Id = Guid.NewGuid(),
            UserId = transaction.UserId,
            WalletId = transaction.WalletId,
            PaymentAddress = transaction.PaymentAddress,
            Topic = transaction.Topic,
            Amount = transaction.Amount,
            AmountReceived = 0,
            Currency = transaction.Currency,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Transactions.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<TransactionEntity> UpdateTransaction(TransactionUpdateModel transaction)
    {
        var entity = await _dbContext.Transactions.FindAsync(transaction.Id);
        
        if (entity is null)
            throw new NotFoundApiException("Transaction not found");

        if (transaction.AmountReceived is not null)
            entity.AmountReceived = transaction.AmountReceived.Value;
        
        if (transaction.Status is not null)
            entity.Status = transaction.Status.Value;
        
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return entity;
    }
}