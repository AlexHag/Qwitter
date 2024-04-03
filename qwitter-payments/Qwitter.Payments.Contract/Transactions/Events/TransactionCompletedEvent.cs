using Qwitter.Core.Application.Kafka;

namespace Qwitter.Payments.Transactions.Models;

[Message("transaction-completed")]
public class TransactionCompletedEvent
{
    public Guid UserId { get; set; }
    public Guid TransactionId { get; set; }
}
