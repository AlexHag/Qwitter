namespace Qwitter.Ledger.Contract.Transactions.Models;

public class CreditFundsRequest
{
    public Guid UserId { get; set; }
    public Guid? AccountId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}