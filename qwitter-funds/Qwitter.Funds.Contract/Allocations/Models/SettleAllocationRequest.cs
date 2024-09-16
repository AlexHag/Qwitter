namespace Qwitter.Funds.Contract.Allocations.Models;

public class SettleAllocationRequest
{
    public Guid AccountId { get; set; }
    public Guid AllocationId { get; set; }
}
