using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.BankAccount.Services;
using Qwitter.Ledger.Contract.BankAccount;
using Qwitter.Ledger.Contract.BankAccount.Models;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.BankAccount;

[ApiController]
[Route("bank-account")]
public class BankAccountController : ControllerBase, IBankAccountController
{
    private readonly IBankAccountService _accountService;

    public BankAccountController(
        IBankAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<BankAccountResponse> CreateBankAccount(CreateBankAccountRequest request)
    {
       return await _accountService.CreateBankAccount(request);
    }

    [HttpGet("{bankAccountId}")]
    public async Task<BankAccountResponse> GetBankAccount(Guid bankAccountId)
    {
        return await _accountService.GetBankAccount(bankAccountId);
    }

    [HttpGet("user/{userId}")]
    public async Task<List<BankAccountResponse>> GetUserBankAccounts(Guid userId)
    {
        return await _accountService.GetUserBankAccounts(userId);
    }

    [HttpPut("user/{userId}/primary-bank-account/{bankAccountId}")]
    public async Task UpdateUserBankPrimaryAccount(Guid userId, Guid bankAccountId)
    {
        await _accountService.UpdateUserPrimaryBankAccount(userId, bankAccountId);
    }
}