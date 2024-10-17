namespace Qwitter.Exchange.Contract.FxRate.Models;

public class UpdateFxRateRequest
{
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal Rate { get; set; }
}