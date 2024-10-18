namespace Qwitter.BankAccounts.Contract.BankAccounts.Models;

public class SetDefaultBankAccountRequest
{
    public Guid UserId { get; set; }
    public Guid BankAccountId { get; set; }
}