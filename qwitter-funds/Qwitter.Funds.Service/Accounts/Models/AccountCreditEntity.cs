namespace Qwitter.Funds.Service.Accounts.Models;

public class AccountCreditEntity
{
    public Guid AccountCreditId { get; set; }
    public Guid AccountId { get; set; }
    public Guid ExternalTransactionId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
}
