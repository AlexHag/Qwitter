namespace Qwitter.Crypto.Contract.Wallet.Models;

public class WalletResponse
{
    public Guid Id { get; set; }
    public required string Currency { get; set; }
    public required string Address { get; set; }
}