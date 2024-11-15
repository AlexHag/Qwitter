namespace Qwitter.Exchange.Contract.FundExchange.Models;

[Flags]
public enum FundExchangeStatus
{
    Initiated = 1,
    DestinationAllocated = 2,
    SourceSettled = 4,
    Completed = 8,
    Failed = 16
}
