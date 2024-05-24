using Qwitter.Ledger.Contract.Invoices.Models;

namespace Qwitter.Ledger.Invoices.Models;

public class InvoiceEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPayed { get; set; }
    public InvoiceStatus Status { get; set; }
    public required string Currency { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}