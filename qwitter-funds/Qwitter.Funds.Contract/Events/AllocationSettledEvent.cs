using Qwitter.Core.Application.Kafka;

namespace Qwitter.Funds.Contract.Events;

[Message("allocation-settled")]
public class AllocationSettledEvent
{
    public Guid AllocationId { get; set; }
}