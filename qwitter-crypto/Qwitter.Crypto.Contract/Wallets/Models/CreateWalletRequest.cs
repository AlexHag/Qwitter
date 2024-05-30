namespace Qwitter.Crypto.Contract.Wallets.Models;

public class CreateWalletRequest
{
    public required string Currency { get; set; }
    public string? SubTopic { get; set; }
}