using Qwitter.Ledger.FundAllocations.Models.Enums;

namespace Qwitter.Ledger.FundAllocations.Models;

public class FundAllocationEntity
{
    public Guid Id { get; set; }
    public Guid SourceId { get; set; }
    public required string SourceDomain { get; set; }
    public decimal SourceAmount { get; set; }
    public required string SourceCurrency { get; set; }

    public Guid? DestinationId { get; set; }
    public string? DestinationDomain { get; set; }
    public decimal? DestinationAmount { get; set; }
    public string? DestinationCurrency { get; set; }

    public decimal? ExchangeRate { get; set; }
    public decimal? Fee { get; set; }

    public FundAllocationStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
