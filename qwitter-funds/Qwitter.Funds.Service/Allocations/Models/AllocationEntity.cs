
using Qwitter.Funds.Contract.Allocations.Enums;

namespace Qwitter.Funds.Service.Allocations.Models;

/// <summary>
///     Represent a tangible entity of funds
/// </summary>
public class AllocationEntity
{
    /// <summary>
    ///     AllocationId of the funds.
    /// </summary>
    public Guid AllocationId { get; set; }
    /// <summary>
    ///     Id of the account funds were allocated from.
    /// </summary>
    public Guid SourceAccountId { get; set; }
    /// <summary>
    ///     Id of the accounts funds were settled into.
    /// </summary>
    public Guid? DestinationAccountId { get; set; }

    /// <summary>
    ///     External Transaction Id of the transaction that allocated the funds from their account
    /// </summary>
    public Guid ExternalSourceTransactionId { get; set; }

    /// <summary>
    ///     External Transaction Id of the transaction that accepted the settled funds into their account
    /// </summary>
    public Guid? ExternalDestinationTransactionId { get; set; }

    /// <summary>
    ///     Currency
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    ///     Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    ///     Status
    /// </summary>
    public AllocationStatus Status { get; set; }

    public DateTime AllocatedAt { get; set; }
    public DateTime? SettledAt { get; set; }
}
