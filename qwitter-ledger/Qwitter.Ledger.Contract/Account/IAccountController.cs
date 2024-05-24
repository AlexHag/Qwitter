using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;

namespace Qwitter.Ledger.Contract.Account;

[ApiHost("5005", "account")]
public interface IAccountController
{
    [HttpPost]
    Task<AccountResponse> CreateLedgerAccount(CreateLedgerAccountRequest request);

    [HttpGet("{accountId}")]
    Task<AccountResponse> GetLedgerAccount(Guid accountId);

    [HttpGet("user/{userId}")]
    Task<List<AccountResponse>> GetUserLedgerAccounts(Guid userId);
}