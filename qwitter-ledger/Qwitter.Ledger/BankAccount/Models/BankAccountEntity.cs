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

    public bool CanTransfer(decimal amount, out string reason)
    {
        if (!IsActive(out reason))
        {
            return false;
        }

        if (Balance < amount && !OverdraftAllowed)
        {
            reason = "Insufficient funds";
            return false;
        }

        reason = string.Empty;
        return true;
    }

    public bool IsActive(out string reason)
    {
        if (AccountStatus == BankAccountStatus.Cancelled)
        {
            reason = "Account is cancelled";
            return false;
        }

        if (AccountStatus == BankAccountStatus.Frozen)
        {
            reason = "Account is frozen";
            return false;
        }

        reason = string.Empty;
        return true;
    }
}