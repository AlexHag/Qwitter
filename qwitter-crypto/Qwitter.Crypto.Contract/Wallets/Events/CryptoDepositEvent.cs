using Qwitter.Core.Application.Kafka;

namespace Qwitter.Crypto.Contract.Wallets.Events;

[Message("crypto-deposit")]
public class CryptoDepositEvent
{
    public Guid WalletId { get; set; }
    public Guid DestinationId { get; set; }
    public required string TransactionHash { get; set; }
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}