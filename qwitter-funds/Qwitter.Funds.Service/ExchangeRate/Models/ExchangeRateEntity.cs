
namespace Qwitter.Funds.Service.ExchangeRate.Models;

public class ExchangeRateEntity
{
    public Guid ExchangeRateId { get; set; }
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal Rate { get; set; }
    public DateTime Created { get; set; }
}