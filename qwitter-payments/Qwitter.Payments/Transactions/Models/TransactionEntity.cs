using Qwitter.Payments.Contract.Transactions.Models;

namespace Qwitter.Payments.Transactions.Models;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid WalletId { get; set; }
    public required string PaymentAddress { get; set; }
    public required string Topic { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountReceived { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}