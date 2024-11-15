
using Qwitter.Exchange.Contract.FundExchange.Models;

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

    public Guid CorrelationId { get; set; }
    public FundExchangeStatus Status { get; set; }

    public Guid SourceAllocationId { get; set; }
    public Guid? DestinationAllocationId { get; set; }

    /// <summary>
    ///     The Funds AccountId that the allocation from the client is settled into.
    /// </summary>
    public Guid SourceCurrencyAccountId { get; set; }

    /// <summary>
    ///     The Funds AccountId where funds are allocated from that can be settled by the client.
    /// </summary>
    public Guid DestinationCurrencyAccountId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
