using System.Text.Json.Serialization;

namespace Qwitter.Ledger.Contract.BankAccount.Models;

public class CreateBankAccountRequest
{
    public Guid UserId { get; set; }
    public string? AccountName { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BankAccountType AccountType { get; set; }
    public required string Currency { get; set; }
}
