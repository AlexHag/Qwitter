namespace Qwitter.Ledger.Contract.Account;

public class AccountResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public required string AccountNumber { get; set; }
    public required string RoutingNumber { get; set; }
    public AccountType AccountType { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
}