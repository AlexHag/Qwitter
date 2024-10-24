
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Crypto.Contract.CryptoTransfer.Models;

namespace Qwitter.Crypto.Contract.CryptoTransfer;

[ApiHost(Host.Name, "crypto-transfer")]
public interface ICryptoTransferService
{
    [HttpPost]
    Task<CreateCryptoTransferResponse> CreateCryptoTransfer(CreateCryptoTransferRequest request);

    [HttpGet("{transactionId}")]
    Task<CryptoTransferResponse> GetTransfer(Guid transactionId);

    [HttpGet("process/{transactionId}")]
    Task Process(Guid transactionId);
}
