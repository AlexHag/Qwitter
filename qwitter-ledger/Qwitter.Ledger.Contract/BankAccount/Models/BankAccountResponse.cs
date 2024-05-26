using System.Text.Json.Serialization;

namespace Qwitter.Ledger.Contract.Account;

public class BankAccountResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    public required string AccountNumber { get; set; }
    public required string RoutingNumber { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BankAccountType AccountType { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
    public bool OverdraftAllowed { get; set; }
    public bool IsPrimary { get; set; }
}