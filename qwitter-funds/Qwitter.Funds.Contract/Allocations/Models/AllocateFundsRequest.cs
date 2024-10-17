
namespace Qwitter.Funds.Contract.Allocations.Models;

public class AllocateFundsRequest
{
    public Guid AccountId { get; set; }
    public Guid TransactionId { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
}