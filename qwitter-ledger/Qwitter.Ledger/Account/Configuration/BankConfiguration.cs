using Qwitter.Core.Application.Configuration;

namespace Qwitter.Ledger.Account.Configuration;

[Configuration("Bank")]
public class BankConfiguration
{
    public required string DefaultRoutingNumber { get; set; }
    public int MaxRecursionDepth { get; set; }
    public int AccountNumberLength { get; set; }
}