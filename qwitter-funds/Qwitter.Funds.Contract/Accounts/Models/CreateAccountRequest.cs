
namespace Qwitter.Funds.Contract.Accounts.Models;

public class CreateAccountRequest
{
    public Guid OwnerReferenceId { get; set; }
    public required string OwnerCallbackTopic { get; set; }
    public required string Currency { get; set; }
}