
namespace Qwitter.Crypto.Contract.CryptoTransfer.Models;

public class CryptoTransferResponse
{
    public Guid TransactionId { get; set; }
    public int? BlockNumber { get; set; }
    public string? BlockHash { get; set; }
    public string? TransactionHash { get; set; }
    public required string SourceAddress { get; set; }
    public required string DestinationAddress { get; set; }
    public decimal Amount { get; set; }
    public decimal Fee { get; set; }
    public required string Currency { get; set; }
    public CryptoTransferStatus Status { get; set; }
}
