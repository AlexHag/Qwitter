
namespace Qwitter.Payments.Contract.Transactions.Models;

// TODO: Add more statuses
public enum TransactionStatus
{
    Pending,
    Completed,
    Expired,
    Withdrawn
}
