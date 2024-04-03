namespace Qwitter.Payments.Transactions.Models;

public class CreateTransactionRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public required string Topic { get; set; }
}