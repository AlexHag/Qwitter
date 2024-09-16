
namespace Qwitter.Funds.Service.CurrencyExchange.Models;

public class CurrencyAccountEntity
{
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
}
