
namespace Qwitter.Ledger.Contract.InternalBankTransfer.Models;

public class InternalBankTransferRequest
{
    public Guid UserId { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
}