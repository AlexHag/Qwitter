
namespace Qwitter.Exchange.Service.CurrencyAccounts.Models;

public class CurrencyAccountEntity
{
    public Guid CurrencyAccountId { get; set; }
    public Guid FundsAccountId { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
}
