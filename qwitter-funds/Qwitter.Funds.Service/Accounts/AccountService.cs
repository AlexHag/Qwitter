using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Funds.Contract.Accounts.Models;
using Qwitter.Funds.Contract.Accounts.Enums;
using Qwitter.Funds.Service.Accounts.Models;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Clients.Repositories;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Accounts;

namespace Qwitter.Funds.Service.Accounts;

[ApiController]
[Route("account")]
[Authorize(AuthenticationSchemes = "mTLS")]
public class AccountService : ControllerBase, IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;

    public AccountService(
        IAccountRepository accountRepository,
        IClientRepository clientRepository,
        ILogger<AccountService> logger,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _clientRepository = clientRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<AccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var existingAccount = await _accountRepository.TryGetByExternalAccountId(request.ExternalAccountId);

        if (existingAccount != null)
        {
            return _mapper.Map<AccountResponse>(existingAccount);
        }

        var thumbprint = HttpContext.Connection.ClientCertificate!.Thumbprint;

        var client = await _clientRepository.GetByThumbprint(thumbprint);

        if (!client.CanAllocateFundsIn && request.AccountType == AccountType.FundsIn)
        {
            throw new ForbiddenApiException("Client Not allowed to create credit accounts");
        }

        if (!client.CanSettleFundsOut && request.AccountType == AccountType.FundsOut)
        {
            throw new ForbiddenApiException("Client Not allowed to create debit accounts");
        }

        var account = new AccountEntity
        {
            AccountId = Guid.NewGuid(),
            ClientId = client.ClientId,
            ExternalAccountId = request.ExternalAccountId,
            AccountType = request.AccountType,
            Currency = request.Currency,
        };

        await _accountRepository.Insert(account);
        return _mapper.Map<AccountResponse>(account);
    }

    [HttpGet]
    public async Task<AccountResponse> GetAccount(Guid accountId)
    {
        var account = await _accountRepository.GetById(accountId);
        return _mapper.Map<AccountResponse>(account);
    }
}