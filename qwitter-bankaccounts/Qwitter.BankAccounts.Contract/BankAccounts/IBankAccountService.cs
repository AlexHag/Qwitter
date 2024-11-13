using Microsoft.AspNetCore.Mvc;
using Qwitter.BankAccounts.Contract.BankAccounts.Models;
using Qwitter.Core.Application.RestApiClient;

namespace Qwitter.BankAccounts.Contract.BankAccounts;

[ApiHost(Host.Name, "bank-account")]
public interface IBankAccountService
{
    [HttpGet("{userId}")]
    Task<List<BankAccountResponse>> GetAccounts(Guid userId);

    [HttpPost("create")]
    Task<BankAccountResponse> CreateBankAccount(CreateBankAccountRequest request);

    [HttpPut("set-default")]
    Task<BankAccountResponse> SetDefaultBankAccount(SetDefaultBankAccountRequest request);
}
