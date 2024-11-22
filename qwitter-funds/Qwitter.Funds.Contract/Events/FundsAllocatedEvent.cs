using Qwitter.Core.Application.Kafka;

namespace Qwitter.Funds.Contract.Events;

[Message("funds-allocated")]
public class FundsAllocatedEvent
{
    public Guid AllocationId { get; set; }
}