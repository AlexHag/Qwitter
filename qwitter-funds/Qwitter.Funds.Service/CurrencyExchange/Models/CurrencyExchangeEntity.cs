
namespace Qwitter.Funds.Service.CurrencyExchange.Models;

public class CurrencyExchangeEntity
{
    public Guid CurrencyExchangeId { get; set; }
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal SourceAmount { get; set; }
    public decimal DestinationAmount { get; set; }
    public Guid ExchangeRateId { get; set; }
    public decimal Rate { get; set; }
}
