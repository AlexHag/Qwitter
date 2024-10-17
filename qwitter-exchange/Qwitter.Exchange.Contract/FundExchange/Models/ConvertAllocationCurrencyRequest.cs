namespace Qwitter.Exchange.Contract.FundExchange.Models;

public class ConvertAllocationCurrencyRequest
{
    public Guid AllocationId { get; set; }
    public Guid FxRateId { get; set; }
}
