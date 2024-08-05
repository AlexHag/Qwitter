namespace Qwitter.Crypto.Contract.Wallets.Models;

public class WalletResponse
{
    public Guid Id { get; set; }
    public required string Currency { get; set; }
    public required string Address { get; set; }
    public required string DestinationDomain { get; set; }
    public Guid DestinationId { get; set; }
}