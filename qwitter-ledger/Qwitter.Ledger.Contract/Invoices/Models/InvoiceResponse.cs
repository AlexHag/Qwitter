namespace Qwitter.Ledger.Contract.Invoices.Models;

public class InvoiceResponse
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