namespace Qwitter.Exchange.Contract.FxRate.Models;

public class FxRateResponse
{
    public Guid FxRateId { get; set; }
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal Rate { get; set; }
}
