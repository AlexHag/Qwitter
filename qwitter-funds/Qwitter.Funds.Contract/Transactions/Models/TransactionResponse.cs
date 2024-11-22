using Qwitter.Funds.Contract.Transactions.Enums;

namespace Qwitter.Funds.Contract.Transactions.Models;

public class TransactionResponse
{
    public Guid AccountId { get; set; }
    public Guid AllocationId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}
