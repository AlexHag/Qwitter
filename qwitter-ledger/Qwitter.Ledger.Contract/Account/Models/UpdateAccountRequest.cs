namespace Qwitter.Ledger.Contract.Account;

public class UpdateAccountRequest
{
    public string? AccountName { get; set; }
    public AccountType AccountType { get; set; }
}