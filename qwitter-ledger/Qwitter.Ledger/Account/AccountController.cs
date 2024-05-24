using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Account.Services;
using Qwitter.Ledger.Contract.Account;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Account;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase, IAccountController
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountController(
        IAccountService accountService,
        IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
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

    [HttpPost("{accountId}/transactions")]
    public async Task<IEnumerable<TransactionResponse>> GetLedgerAccountTransactions(Guid accountId, PaginationRequest request)
    {
        return await _accountService.GetLedgerAccountTransactions(accountId, request);
    }

    [HttpGet("user/{userId}")]
    public async Task<List<AccountResponse>> GetUserLedgerAccounts(Guid userId)
    {
        return await _accountService.GetUserLedgerAccounts(userId);
    }

    [HttpPut("user/{userId}/primary-account/{accountId}")]
    public async Task UpdateUserPrimaryAccount(Guid userId, Guid accountId)
    {
        await _accountService.UpdateUserPrimaryAccount(userId, accountId);
    }
}