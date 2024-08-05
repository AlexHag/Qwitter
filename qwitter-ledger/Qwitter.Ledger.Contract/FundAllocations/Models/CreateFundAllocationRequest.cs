namespace Qwitter.Ledger.Contract.FundAllocations.Models;

public class CreateFundAllocationRequest
{
    public Guid SourceId { get; set; }
    public required string SourceTopic { get; set; }
    public Guid DestinationId { get; set; }
    public required string DestinationTopic { get; set; }

    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}
