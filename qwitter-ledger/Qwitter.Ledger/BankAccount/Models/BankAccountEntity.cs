using Qwitter.Ledger.Contract.BankAccount.Models;

namespace Qwitter.Ledger.BankAccount.Models;

public class BankAccountEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public required string AccountNumber { get; set; }
    public required string RoutingNumber { get; set; }
    public BankAccountType AccountType { get; set; }
    public BankAccountStatus AccountStatus { get; set; }
    public decimal Balance { get; set; }
    public required string Currency { get; set; }
    public bool OverdraftAllowed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}