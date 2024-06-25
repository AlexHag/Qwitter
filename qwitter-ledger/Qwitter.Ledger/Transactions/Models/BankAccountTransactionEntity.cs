namespace Qwitter.Ledger.Transactions.Models;

public class BankAccountTransactionEntity
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid AllocationId { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
    public decimal Amount { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// TODO Add AccountTransactionStatus - Accepted, Declined, Pending, Canceled, Reversed
// TODO Add destinations
