
namespace Qwitter.Funds.Contract.Accounts.Models;

public class CreateAccountRequest
{
    public Guid AccountId { get; set; }
    public required string Currency { get; set; }
}