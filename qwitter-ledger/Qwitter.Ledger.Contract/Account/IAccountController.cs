using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Contract.Account;

[ApiHost("5005", "account")]
public interface IAccountController
{
    [HttpPost]
    Task<AccountResponse> CreateLedgerAccount(CreateLedgerAccountRequest request);

    [HttpGet("{accountId}")]
    Task<AccountResponse> GetLedgerAccount(Guid accountId);

    [HttpPost("{accountId}/transactions")]
    Task<IEnumerable<TransactionResponse>> GetLedgerAccountTransactions(Guid accountId, PaginationRequest request);

    [HttpGet("user/{userId}")]
    Task<List<AccountResponse>> GetUserLedgerAccounts(Guid userId);

    [HttpPut("user/{userId}/primary-account/{accountId}")]
    Task UpdateUserPrimaryAccount(Guid userId, Guid accountId);
}