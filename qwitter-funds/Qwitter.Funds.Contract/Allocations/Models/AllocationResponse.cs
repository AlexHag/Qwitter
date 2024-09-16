
using Qwitter.Funds.Contract.Allocations.Enums;

namespace Qwitter.Funds.Contract.Allocations.Models;

public class AllocationResponse
{
   public Guid AllocationId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }

    public AllocationStatus Status { get; set; }

    public Guid SourceAccountId { get; set; }
    public Guid? DestinationAccountId { get; set; }

    public Guid? ConvertedIntoAllocationId { get; set; }
    public Guid? CurrencyExchangeId { get; set; }
}
