namespace Qwitter.Payments.Wallets.Services;

public class WalletEntity
{
    public Guid Id { get; set; }
    public required string Currency { get; set; }
    public required string Address { get; set; }
    public required string PrivateKey { get; set; }
}