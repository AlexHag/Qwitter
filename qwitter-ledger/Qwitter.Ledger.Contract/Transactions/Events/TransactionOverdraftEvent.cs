using Qwitter.Core.Application.Kafka;

namespace Qwitter.Ledger.Contract.Transactions.Events;

[Message("transaction-overdraft")]
public class TransactionOverdraftEvent
{
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public Guid TransactionId { get; set; }
}