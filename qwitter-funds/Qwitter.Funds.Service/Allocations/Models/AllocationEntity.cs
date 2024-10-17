
using Qwitter.Funds.Contract.Allocations.Enums;

namespace Qwitter.Funds.Service.Allocations.Models;

public class AllocationEntity
{
    public Guid AllocationId { get; set; }
    public Guid AccountId { get; set; }
    public Guid TransactionId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
    public AllocationStatus Status { get; set; }
    public Guid? DestinationAccountId { get; set; }
}
