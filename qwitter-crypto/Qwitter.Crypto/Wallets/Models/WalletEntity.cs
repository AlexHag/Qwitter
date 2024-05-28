namespace Qwitter.Crypto.Wallets.Services;

public class WalletEntity
{
    public Guid Id { get; set; }
    public required string Currency { get; set; }
    public required string Address { get; set; }
    public decimal Balance { get; set; }
    public string? PrivateKey { get; set; }
}