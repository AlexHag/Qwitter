namespace Qwitter.Crypto.Currency.Contract.Wallets.Models;

public class WalletModel
{
    public required string Currency { get; set; }
    public required string Address { get; set; }
    public string? PrivateKey { get; set; }
}