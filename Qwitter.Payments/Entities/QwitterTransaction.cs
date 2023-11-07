
namespace Qwitter.Payments.Entities;

public class QwitterTransaction
{
    public Guid Id { get; set; }
    public required string FromAddress { get; set; }
    public required string ToAddress { get; set; }
    public decimal Amount { get; set; }
    public string? TransactionHash { get; set; }
    public QwitterTransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public enum QwitterTransactionStatus
{
    Unknown = 1,
    Pending = 2,
    Completed = 3,
    Canceled = 4,
    Failed = 5
}
