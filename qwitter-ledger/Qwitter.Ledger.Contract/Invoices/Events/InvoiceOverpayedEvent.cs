using Qwitter.Core.Application.Kafka;

namespace Qwitter.Ledger.Contract.Invoices.Models;

[Message("invoice-overpayed")]
public class InvoiceOverpayedEvent
{
    public Guid UserId { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TransactionId { get; set; }
    public Guid InvoicePaymentId { get; set; }
}