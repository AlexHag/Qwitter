namespace Qwitter.Ledger.Contract.Transactions.Models;

public class TransferFundsRequest
{
    public Guid FromBankAcountId { get; set; }
    public Guid ToBankAccountId { get; set; }
    public decimal Amount { get; set; }
    public string? Message { get; set; }
}