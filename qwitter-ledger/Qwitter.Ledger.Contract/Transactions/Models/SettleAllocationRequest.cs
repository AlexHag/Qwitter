namespace Qwitter.Ledger.Contract.Transactions.Models;

public class SettleAllocationRequest
{
    public Guid FundAllocationId { get; set; }
    public Guid BankAccountId { get; set; }
}