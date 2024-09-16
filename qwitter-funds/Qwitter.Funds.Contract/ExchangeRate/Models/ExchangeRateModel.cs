
namespace Qwitter.Funds.Contract.ExchangeRate.Models;

public class ExchangeRateModel
{
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal Rate { get; set; }
}