namespace Qwitter.Crypto.Service.CryptoTransfer.Models;

public class CryptoOutWalletEntity
{
    public Guid Id { get; set; }
    public required string Currency { get; set; }
    public Guid WalletId { get; set; }
}