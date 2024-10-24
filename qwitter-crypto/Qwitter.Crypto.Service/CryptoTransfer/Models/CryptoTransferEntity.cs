using Qwitter.Crypto.Contract.CryptoTransfer.Models;

namespace Qwitter.Crypto.Service.CryptoTransfer.Models;

public class CryptoTransferEntity
{
    public Guid TransactionId { get; set; }
    public int? BlockNumber { get; set; }
    public string? BlockHash { get; set; }
    public string? TransactionHash { get; set; }
    public required string SourceAddress { get; set; }
    public required string DestinationAddress { get; set; }
    public decimal Amount { get; set; }
    public decimal? Fee { get; set; }
    public required string Currency { get; set; }
    public string? SubTopic { get; set; }
    public CryptoTransferStatus Status { get; set; }
}
