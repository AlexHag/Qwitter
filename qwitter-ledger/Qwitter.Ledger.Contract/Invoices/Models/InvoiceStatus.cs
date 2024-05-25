namespace Qwitter.Ledger.Contract.Invoices.Models;

public enum InvoiceStatus
{
    Pending,
    PartiallyPaid,
    Paid,
    Overdue,
    Cancelled
}