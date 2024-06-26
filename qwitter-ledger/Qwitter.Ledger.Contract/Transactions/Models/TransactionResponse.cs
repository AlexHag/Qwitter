namespace Qwitter.Ledger.Contract.Transactions.Models;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal NewBalance { get; set; }
    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }
    public decimal SourceAmount { get; set; }
    public decimal DestinationAmount { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal? Fee { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
}