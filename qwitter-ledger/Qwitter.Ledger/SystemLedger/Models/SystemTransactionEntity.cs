using Qwitter.Core.Application.Persistence;

namespace Qwitter.Ledger.SystemLedger.Models;

public class SystemTransactionEntity
{
    public Guid Id { get; set; }
    public Guid FundAllocationId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}
