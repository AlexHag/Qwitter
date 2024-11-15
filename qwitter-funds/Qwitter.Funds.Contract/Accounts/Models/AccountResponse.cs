using Qwitter.Funds.Contract.Accounts.Enums;

namespace Qwitter.Funds.Contract.Accounts.Models;

public class AccountResponse
{
    public Guid AccountId { get; set; }
    public Guid ExternalAccountId { get; set; }
    public required string Currency { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Balance { get; set; }
}
