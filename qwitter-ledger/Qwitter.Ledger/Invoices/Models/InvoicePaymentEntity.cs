using Qwitter.Ledger.Invoices.Models;

public class InvoicePaymentEntity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public DateTime CreatedAt { get; set; }
}