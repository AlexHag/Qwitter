
namespace Qwitter.Funds.Contract.Accounts.Models;

public class CreditAccountRequest
{
    public Guid AccountId { get; set; }
    public Guid TransactionId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
}
