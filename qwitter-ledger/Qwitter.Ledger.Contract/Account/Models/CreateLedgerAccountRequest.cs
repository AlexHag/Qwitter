namespace Qwitter.Ledger.Contract.Account;

public class CreateBankAccountRequest
{
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public BankAccountType AccountType { get; set; }
    public required string Currency { get; set; }
}
