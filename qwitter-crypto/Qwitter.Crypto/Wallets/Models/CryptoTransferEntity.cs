
namespace Qwitter.Crypto.Wallets.Models;

public class CryptoTransferEntity
{
    public Guid Id { get; set; }
    public int BlockNumber { get; set; }
    public string? BlockHash { get; set; }
    public required string TransactionHash { get; set; }
    public required string From { get; set; }
    public required string To { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}