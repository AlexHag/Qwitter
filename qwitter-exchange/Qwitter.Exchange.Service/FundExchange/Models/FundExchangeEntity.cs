
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

    public FundExchangeStatus Status { get; set; }

    public Guid SourceAllocationId { get; set; }
    public Guid? DestinationAllocationId { get; set; }

    /// <summary>
    ///     The account where the funds from the client are settled into.
    /// </summary>
    public Guid SourceCurrencyAccountId { get; set; }

    /// <summary>
    ///     The account where the funds to the client are allocated from.
    /// </summary>
    public Guid DestinationCurrencyAccountId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
