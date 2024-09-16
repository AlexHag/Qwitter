
namespace Qwitter.Funds.Service.Accounts.Models;

public class AccountEntity
{
    public Guid AccountId { get; set; }
    public Guid OwnerReferenceId { get; set; }
    public required string OwnerCallbackTopic { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
}
