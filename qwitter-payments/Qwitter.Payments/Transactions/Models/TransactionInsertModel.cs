
namespace Qwitter.Payments.Transactions.Models;

public class TransactionInsertModel
{
    public Guid UserId { get; set; }
    public Guid WalletId { get; set; }
    public required string PaymentAddress { get; set; }
    public required string Topic { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}