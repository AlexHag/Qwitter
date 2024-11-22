using Qwitter.Funds.Contract.Transactions.Enums;

namespace Qwitter.Funds.Service.Transactions.Models;

public class TransactionEntity
{
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    public Guid AllocationId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}
