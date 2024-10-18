
namespace Qwitter.BankAccounts.Contract.BankAccounts.Models;

public class CreateBankAccountRequest
{
    public Guid UserId { get; set; }
    public required string Currency { get; set; }
}
