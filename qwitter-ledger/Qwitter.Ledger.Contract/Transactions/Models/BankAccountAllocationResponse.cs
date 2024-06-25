namespace Qwitter.Ledger.Contract.Transactions.Models;

public class BankAccountAllocationResponse
{
    public required BankAccountTransaction Transaction { get; set; }
    public required FundAllocation Allocation { get; set; }
}