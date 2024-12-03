namespace Qwitter.Exchange.Contract.FxRate.Models;

public class ValidateFxRateRequest
{
    public decimal SourceAmount { get; set; }
    public decimal DestinationAmount { get; set; }
    public Guid FxRateId { get; set; }
}