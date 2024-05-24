namespace Qwitter.Ledger.Contract.Account;

public class AccountModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public required string AccountNumber { get; set; }
    public BankAccountType AccountType { get; set; }
    public BankAccountStatus AccountStatus { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}