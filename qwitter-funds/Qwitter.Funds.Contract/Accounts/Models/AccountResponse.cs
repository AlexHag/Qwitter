
namespace Qwitter.Funds.Contract.Accounts.Models;

public class AccountResponse
{
    public Guid AccountId { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
}
