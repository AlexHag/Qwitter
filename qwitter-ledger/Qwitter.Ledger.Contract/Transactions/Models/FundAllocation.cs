namespace Qwitter.Ledger.Contract.Transactions.Models;

public class FundAllocation
{
    public Guid Id { get; set; }
    public decimal SourceAmount { get; set; }
    public decimal? DestinationAmount { get; set; }
    public required string SourceCurrency { get; set; }
    public string? DestinationCurrency { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? Fee { get; set; }
    public required string Status { get; set; }
    public required string Source { get; set; }
    public Guid SourceReferenceId { get; set; }
    public string? Destination { get; set; }
    public Guid? DestinationReferenceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}