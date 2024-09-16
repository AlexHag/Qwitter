
namespace Qwitter.Funds.Service.Accounts.Models;

public class AccountCreditEntity
{
    public Guid AccountCreditId { get; set; }
    public required string SourceTransactionUrl { get; set; }
    public Guid SourceTransactionReferenceId { get; set; }
    public decimal Amount { get; set; }
}