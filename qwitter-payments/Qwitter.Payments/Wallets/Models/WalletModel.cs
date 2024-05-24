
namespace Qwitter.Payments.Wallets.Models;

public class WalletModel
{
    public Guid Id { get; set; }
    public required string Currency { get; set; }
    public required string Address { get; set; }
}