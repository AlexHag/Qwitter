namespace Qwitter.Crypto.Contract.Wallets.Models;

public class CreateWalletRequest
{
    public required string Currency { get; set; }
    public required string DestinationDomain { get; set; }
    public Guid DestinationId { get; set; }
}