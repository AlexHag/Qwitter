using Qwitter.Core.Application.Kafka;

namespace Qwitter.Crypto.Contract.Wallet.Events;

[Message("crypto-deposit")]
public class CryptoDepositEvent
{
    public Guid WalletId { get; set; }
    public required string TransactionHash { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}