using Microsoft.AspNetCore.Mvc;
using Qwitter.Ledger.Account.Services;
using Qwitter.Ledger.Contract.Account;

namespace Qwitter.Ledger.Account;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase, IAccountController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<AccountResponse> CreateLedgerAccount(CreateLedgerAccountRequest request)
    {
       return await _accountService.CreateLedgerAccount(request);
    }

    [HttpGet("{accountId}")]
    public async Task<AccountResponse> GetLedgerAccount(Guid accountId)
    {
        return await _accountService.GetLedgerAccount(accountId);
    }

    [HttpGet("user/{userId}")]
    public async Task<List<AccountResponse>> GetUserLedgerAccounts(Guid userId)
    {
        return await _accountService.GetUserLedgerAccounts(userId);
    }
}