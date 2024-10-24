
namespace Qwitter.Crypto.Contract.CryptoTransfer.Models;

public class CreateCryptoTransferRequest
{
    public required string Address { get; set; }
    public required string Currency { get; set; }
    public decimal Amount { get; set; }
    public string? SubTopic { get; set; }
}
