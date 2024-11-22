namespace Qwitter.Funds.Service.Transactions.Models;

public class TransactionCommand
{
    public Guid AccountId { get; set; }
    public Guid AllocationId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
}