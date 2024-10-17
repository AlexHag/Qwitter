
using Qwitter.Funds.Contract.Allocations.Enums;

namespace Qwitter.Funds.Contract.Allocations.Models;

public class AllocationResponse
{
    public Guid AllocationId { get; set; }
    public Guid AccountId { get; set; }
    public Guid TransactionId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
    public AllocationStatus Status { get; set; }
    public Guid? SettlementAccountId { get; set; }
}
