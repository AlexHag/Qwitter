
namespace Qwitter.Funds.Contract.Allocations.Models;

public class AllocateFundsRequest
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
}