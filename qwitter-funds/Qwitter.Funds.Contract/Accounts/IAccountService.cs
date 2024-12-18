
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Accounts.Models;

namespace Qwitter.Funds.Contract.Accounts;

[ApiHost(Host.Name, "account")]
public interface IAccountService
{
    [HttpPost("create")]
    Task<AccountResponse> CreateAccount(CreateAccountRequest request);

    [HttpGet("{accountId}")]
    Task<AccountResponse> GetAccount(Guid accountId);
}
