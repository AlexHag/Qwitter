namespace Qwitter.Ledger.ExchangeRates.Models;

public class ExchangeRateEntity
{
    public Guid Id { get; set; }
    public required string Source { get; set; }
    public required string Destination { get; set; }
    public decimal Rate { get; set; }
}