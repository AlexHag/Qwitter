namespace Qwitter.Ledger.Contract.Invoices.Models;

public class PayInvoiceRequest
{
    public Guid UserId { get; set; }
    public Guid? BankAccountId { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public string? Message { get; set; }
}