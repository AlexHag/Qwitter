using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Transactions.Models;

namespace Qwitter.Funds.Contract.Transactions;

[ApiHost(Host.Name, "transactions")]
public interface ITransactionService
{
    [HttpPost("filter")]
    Task<PaginationResponse<TransactionResponse>> GetTransactions(PaginationRequest<TransactionFilterRequest> request);
}
