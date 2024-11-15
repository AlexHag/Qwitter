
using Qwitter.Funds.Contract.Accounts.Enums;

namespace Qwitter.Funds.Service.Accounts.Models;

public class AccountEntity
{
    public Guid AccountId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ExternalAccountId { get; set; }
    public AccountType AccountType { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
