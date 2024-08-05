namespace Qwitter.Ledger.Contract.Transactions.Models;

public class AllocateFundsRequest
{
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
    // TODO: Add destination
}