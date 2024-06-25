namespace Qwitter.Ledger.Contract.Transactions.Models;

public class BankAccountTransaction
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid AllocationId { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}