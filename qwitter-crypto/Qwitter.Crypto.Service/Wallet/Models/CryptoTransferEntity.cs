namespace Qwitter.Crypto.Service.Wallet.Models;

public class CryptoTransferEntity
{
    public Guid Id { get; set; }
    public int BlockNumber { get; set; }
    public string? BlockHash { get; set; }
    public required string TransactionHash { get; set; }
    public required string SourceAddress { get; set; }
    public required string DestinationAddress { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}
