
namespace Qwitter.Ledger.Crypto.Models;

public class BankAccountCryptoWalletEntity
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Guid BankAccountId { get; set; }
    public required string Address { get; set; }
    public required string Currency { get; set; }
}
