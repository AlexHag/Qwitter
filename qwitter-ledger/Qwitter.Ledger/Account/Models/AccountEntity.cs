using Qwitter.Ledger.Contract.Account;

namespace Qwitter.Ledger.Account.Models;

public class AccountEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public required string AccountNumber { get; set; }
    public required string RoutingNumber { get; set; }
    public AccountType AccountType { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public decimal Balance { get; set; }
    public required string Currency { get; set; }
    public bool OverdraftAllowed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}