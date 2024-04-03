namespace Qwitter.Payments.Contract.Transactions.Models;

public class CreateTransactionResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public required string Topic { get; set; }
    public required string PaymentAddress { get; set; }
}
