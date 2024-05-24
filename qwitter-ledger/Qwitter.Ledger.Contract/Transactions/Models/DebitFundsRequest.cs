namespace Qwitter.Ledger.Contract.Transactions.Models;

public class DebitFundsRequest
{
    public Guid UserId { get; set; }
    public Guid? BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
    public string? Message { get; set; }
}