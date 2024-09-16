
namespace Qwitter.Funds.Contract.Allocations.Models;

public class ConvertAllocationRequest
{
    public Guid AllocationId { get; set; }
    public required string Currency { get; set; }
}
