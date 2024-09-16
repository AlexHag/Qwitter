
namespace Qwitter.Funds.Service.CurrencyExchange.Models;

public class ExchangeCurrencyCommand
{
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal Amount { get; set; }
}
