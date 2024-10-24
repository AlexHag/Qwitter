using Qwitter.Core.Application.Kafka;

namespace Qwitter.Crypto.Contract.CryptoTransfer.Events;

[Message("crypto-transfer-status-updated")]
public class CryptoTransferStatusUpdatedEvent
{
    public Guid TransactionId { get; set; }
}