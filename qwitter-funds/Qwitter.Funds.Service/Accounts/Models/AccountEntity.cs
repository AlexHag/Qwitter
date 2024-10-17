
namespace Qwitter.Funds.Service.Accounts.Models;

public class AccountEntity
{
    public Guid AccountId { get; set; }
    public required string Currency { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal TotalBalance { get; set; }
}
