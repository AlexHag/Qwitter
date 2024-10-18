namespace Qwitter.BankAccounts.Contract.BankAccounts.Models;

public class BankAccountResponse
{
    public Guid BankAccountId { get; set; }
    public Guid UserId { get; set; }
    public required string AccountNumber { get; set; }
    public required string Currency { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal TotalBalance { get; set; }
    public bool IsDefault { get; set; }
}
