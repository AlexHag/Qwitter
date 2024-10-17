
namespace Qwitter.Exchange.Service.FundExchange.Models;

public class FundExchangeEntity
{
    public Guid TransactionId { get; set; }

    public required string SourceCurrency { get; set; }
    public required string DestinationCurrency { get; set; }

    public Guid FxRateId { get; set; }
    public decimal Rate { get; set; }

    public decimal SourceAmount { get; set; }
    public decimal DestinationAmount { get; set; }

    public Guid SourceAllocationId { get; set; }
    public Guid DestinationAllocationId { get; set; }
}
