namespace Qwitter.BankAccounts.Service.BankAccounts.Models;

public class BankAccountEntity
{
    public Guid BankAccountId { get; set; }
    public Guid UserId { get; set; }
    public required string AccountNumber { get; set; }
    public required string Currency { get; set; }
    public required decimal AvailableBalance { get; set; }
    public required decimal TotalBalance { get; set; }
    public bool IsDefault { get; set; }
    public DateTime Created { get; set; }
}
