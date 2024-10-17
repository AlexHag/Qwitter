namespace Qwitter.Exchange.Service.Rate.Models;

public class FxRateEntity
{
    public Guid FxRateId { get; set; }
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal Rate { get; set; }
    public DateTime Created { get; set; }
}