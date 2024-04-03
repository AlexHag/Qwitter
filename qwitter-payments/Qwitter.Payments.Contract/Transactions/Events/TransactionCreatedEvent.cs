using Qwitter.Core.Application.Kafka;

namespace Qwitter.Payments.Transactions.Models;

[Message("transaction-created")]
public class TransactionCreatedEvent
{
    public Guid UserId { get; set; }
    public Guid TransactionId { get; set; }
}
