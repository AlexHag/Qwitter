using Qwitter.Core.Application.Kafka;

namespace Qwitter.Payments.Contract.Transactions.Events;

[Message("transaction-completed")]
public class TransactionCompletedEvent
{
    public Guid UserId { get; set; }
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}
