using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Invoices.Models;

namespace Qwitter.Ledger.Invoices.Repositories;

public interface IInvoiceRepository
{
    Task Insert(InvoiceEntity entity);
    Task Update(InvoiceEntity entity);
    Task<InvoiceEntity?> GetById(Guid id);
    Task<IEnumerable<InvoiceEntity>> GetByUserId(Guid userId, PaginationRequest request);
}

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _dbContext;

    public InvoiceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(InvoiceEntity entity)
    {
        await _dbContext.Invoices.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(InvoiceEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbContext.Invoices.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<InvoiceEntity?> GetById(Guid id)
    {
        return await _dbContext.Invoices.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InvoiceEntity>> GetByUserId(Guid userId, PaginationRequest request)
    {
        return await _dbContext.Invoices
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .Skip(request.Offset)
            .Take(request.Take)
            .ToListAsync();
    }
}