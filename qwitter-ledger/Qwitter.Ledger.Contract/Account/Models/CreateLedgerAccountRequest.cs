namespace Qwitter.Ledger.Contract.Account;

public class CreateLedgerAccountRequest
{
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public AccountType AccountType { get; set; }
    public required string Currency { get; set; }
}
