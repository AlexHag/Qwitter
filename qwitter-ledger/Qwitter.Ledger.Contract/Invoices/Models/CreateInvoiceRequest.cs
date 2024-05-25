namespace Qwitter.Ledger.Contract.Invoices.Models;

public class CreateInvoiceRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
}