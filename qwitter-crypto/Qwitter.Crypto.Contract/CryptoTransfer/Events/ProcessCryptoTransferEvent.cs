using Qwitter.Core.Application.Kafka;

namespace Qwitter.Crypto.Contract.CryptoTransfer.Events;

[Message("process-crypto-transfer")]
public class ProcessCryptoTransferEvent
{
    public Guid TransactionId { get; set; }
}
