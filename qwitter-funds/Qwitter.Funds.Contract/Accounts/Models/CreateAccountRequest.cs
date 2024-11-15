using Qwitter.Funds.Contract.Accounts.Enums;

namespace Qwitter.Funds.Contract.Accounts.Models;

public class CreateAccountRequest
{
    public AccountType AccountType { get; set; }
    public Guid ExternalAccountId { get; set; }
    public required string Currency { get; set; }
}