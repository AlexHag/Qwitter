using MapsterMapper;
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
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;

    public AccountService(
        IAccountRepository accountRepository,
        IAccountCreditRepository accountCreditRepository,
        ILogger<AccountService> logger,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _accountCreditRepository = accountCreditRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<AccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var existingAccount = await _accountRepository.TryGetById(request.AccountId);

        if (existingAccount != null)
        {
            _logger.LogWarning("Duplicate account creation {AccountId}", request.AccountId);
            return _mapper.Map<AccountResponse>(existingAccount);
        }

        var account = _mapper.Map<AccountEntity>(request);

        await _accountRepository.Insert(account);

        return _mapper.Map<AccountResponse>(account);
    }

    [HttpPost("credit")]
    public async Task<CreditAccountResponse> Credit(CreditAccountRequest request)
    {
        var account = await _accountRepository.GetById(request.AccountId);

        var duplicateCredit = await _accountCreditRepository.TryGetByExternalTransactionId(request.TransactionId);

        if (duplicateCredit != null)
        {
            _logger.LogWarning("Duplicate credit transaction {TransactionId}", request.TransactionId);

            return new CreditAccountResponse
            {
                AccountCreditId = duplicateCredit.AccountCreditId,
            };
        }

        if (request.Currency != account.Currency)
        {
            _logger.LogError("Currency mismatch {AccountId} {TransactionId} {Currency} {AccountCurrency} for credit", request.AccountId, request.TransactionId, request.Currency, account.Currency);

            throw new BadRequestApiException("Currency mismatch");
        }

        var accountCredit = new AccountCreditEntity
        {
            AccountCreditId = Guid.NewGuid(),
            AccountId = request.AccountId,
            ExternalTransactionId = request.TransactionId,
            Currency = request.Currency,
            Amount = request.Amount,
        };

        account.AvailableBalance += accountCredit.Amount;
        account.TotalBalance += accountCredit.Amount;

        await _accountRepository.Update(account);
        await _accountCreditRepository.Insert(accountCredit);

        return new CreditAccountResponse
        {
            AccountCreditId = accountCredit.AccountCreditId,
        };
    }

    [HttpGet]
    public async Task<AccountResponse> GetAccount(Guid accountId)
    {
        var account = await _accountRepository.GetById(accountId);
        return _mapper.Map<AccountResponse>(account);
    }
}