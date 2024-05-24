using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Persistence;

namespace Qwitter.Ledger.Invoices.Repositories;

public interface IInvoicePaymentRepository
{
    Task Insert(InvoicePaymentEntity invoicePayment);
    Task<InvoicePaymentEntity?> GetById(Guid invoicePaymentId);
    Task<IEnumerable<InvoicePaymentEntity>> GetByInvoiceId(Guid invoiceId, PaginationRequest request);
}

public class InvoicePaymentRepository : IInvoicePaymentRepository
{
    private readonly AppDbContext _dbContext;

    public InvoicePaymentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InvoicePaymentEntity?> GetById(Guid invoicePaymentId)
    {
        return await _dbContext.InvoicePayments.FindAsync(invoicePaymentId);
    }

    public async Task<IEnumerable<InvoicePaymentEntity>> GetByInvoiceId(Guid invoiceId, PaginationRequest request)
    {
        return await _dbContext.InvoicePayments
            .Where(x => x.InvoiceId == invoiceId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(request.Offset)
            .Take(request.Take)
            .ToListAsync();
    }

    public async Task Insert(InvoicePaymentEntity invoicePayment)
    {
        await _dbContext.InvoicePayments.AddAsync(invoicePayment);
        await _dbContext.SaveChangesAsync();
    }
}