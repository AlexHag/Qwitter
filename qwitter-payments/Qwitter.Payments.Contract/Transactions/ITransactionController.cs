using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Transactions.Models;

namespace Qwitter.Payments.Contract.Transactions;

[ApiHost("5003", "transactions")]
public interface ITransactionController
{
    [HttpPost("create")]
    Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request);

    [HttpPost("sync/{transactionId}")]
    Task SyncTransaction(Guid transactionId);
}
