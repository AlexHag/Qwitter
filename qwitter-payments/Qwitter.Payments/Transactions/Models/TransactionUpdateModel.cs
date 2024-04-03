using Qwitter.Payments.Contract.Transactions.Models;

namespace Qwitter.Payments.Transactions.Models;

public class TransactionUpdateModel
{
    public Guid Id { get; set; }
    public decimal? AmountReceived { get; set; }
    public TransactionStatus? Status { get; set; }
}