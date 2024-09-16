
namespace Qwitter.Funds.Contract.Accounts.Models;

public class CreditAccountRequest
{
    public Guid AccountId { get; set; }
    public required string SourceTransactionUrl { get; set; }
    public Guid SourceTransactionReferenceId { get; set; }
    public decimal Amount { get; set; }
}
