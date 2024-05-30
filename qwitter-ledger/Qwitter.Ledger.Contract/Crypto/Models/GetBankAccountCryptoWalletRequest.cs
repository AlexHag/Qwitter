namespace Qwitter.Ledger.Contract.Crypto.Models;

public class GetBankAccountCryptoWalletRequest
{
    public Guid UserId { get; set; }
    public Guid BankAccountId { get; set; }
    public required string Currency { get; set; }
}