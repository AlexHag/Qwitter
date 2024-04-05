using Qwitter.Core.Application.Configuration;

namespace Qwitter.Payments.Transactions.Configuration;

[Configuration("Transactions")]
public class TransactionConfiguration
{
    public required string WithdrawingAddress { get; set; }
}