namespace Qwitter.Funds.Contract.Allocations.Models;

public class SettleAllocationRequest
{
    public Guid AllocationId { get; set; }
    public Guid DestinationAccountId { get; set; }
    public Guid ExternalTransactionId { get; set; }
}
