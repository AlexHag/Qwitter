
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Accounts.Models;
using Qwitter.Funds.Service.Accounts.Models;
using Qwitter.Funds.Service.Accounts.Repositories;

namespace Qwitter.Funds.Contract.Accounts;

[ApiController]
[Route("account")]
public class AccountService : ControllerBase, IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountCreditRepository _accountCreditRepository;

    public AccountService(
        IAccountRepository accountRepository,
        IAccountCreditRepository accountCreditRepository)
    {
        _accountRepository = accountRepository;
        _accountCreditRepository = accountCreditRepository;
    }

    [HttpPost("create")]
    public async Task<AccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var existingAccount = await _accountRepository.TryGetByOwnerReferenceIdAndCurrency(request.OwnerReferenceId, request.Currency);

        if (existingAccount != null)
        {
            throw new ConflictApiException("Account already exists");
        }

        var account = new AccountEntity
        {
            AccountId = Guid.NewGuid(),
            OwnerReferenceId = request.OwnerReferenceId,
            OwnerCallbackTopic = request.OwnerCallbackTopic,
            Currency = request.Currency,
            Balance = 0,
        };

        await _accountRepository.Insert(account);

        return new AccountResponse
        {
            AccountId = account.AccountId,
            Currency = account.Currency,
            Balance = account.Balance,
        };
    }

    [HttpPost("credit")]
    public async Task<CreditAccountResponse> Credit(CreditAccountRequest request)
    {
        var account = await _accountRepository.GetById(request.AccountId);

        var accountCredit = new AccountCreditEntity
        {
            AccountCreditId = Guid.NewGuid(),
            SourceTransactionUrl = request.SourceTransactionUrl,
            SourceTransactionReferenceId = request.SourceTransactionReferenceId,
            Amount = request.Amount,
        };

        // TODO: Get transaction and validate

        account.Balance += accountCredit.Amount;

        await _accountRepository.Update(account);
        await _accountCreditRepository.Insert(accountCredit);

        // TODO: Publish event

        return new CreditAccountResponse
        {
            AccountCreditId = accountCredit.AccountCreditId,
        };
    }

    [HttpGet]
    public async Task<AccountResponse> GetAccount(Guid accountId)
    {
        var account = await _accountRepository.GetById(accountId);

        return new AccountResponse
        {
            AccountId = account.AccountId,
            Currency = account.Currency,
            Balance = account.Balance,
        };
    }
}