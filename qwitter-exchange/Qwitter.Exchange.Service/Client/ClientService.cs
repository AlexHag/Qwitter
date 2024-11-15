using Microsoft.AspNetCore.Mvc;
using Qwitter.Exchange.Service.CurrencyAccounts;
using Qwitter.Exchange.Service.CurrencyAccounts.Models;
using Qwitter.Exchange.Service.CurrencyAccounts.Repositories;
using Qwitter.Funds.Contract.Accounts;
using Qwitter.Funds.Contract.Accounts.Enums;
using Qwitter.Funds.Contract.Accounts.Models;
using Qwitter.Funds.Contract.Allocations;
using Qwitter.Funds.Contract.Allocations.Models;
using Qwitter.Funds.Contract.Clients;
using Qwitter.Funds.Contract.Clients.Models;

namespace Qwitter.Exchange.Service.Client;

[ApiController]
[Route("client")]
public class ClientService : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IAccountService _accountService;
    private readonly IAllocationService _allocationService;
    private readonly ICurrencyAccountRepository _currencyAccountRepository;

    public ClientService(
        IClientService clientService,
        IAccountService accountService,
        IAllocationService allocationService,
        ICurrencyAccountRepository currencyAccountRepository)
    {
        _clientService = clientService;
        _accountService = accountService;
        _allocationService = allocationService;
        _currencyAccountRepository = currencyAccountRepository;
    }

    [HttpPost("register")]
    public async Task Register()
    {
        await _clientService.Register(new RegisterAsClientRequest { CallbackUrl = "https://localhost:5204" });
    }

    [HttpPost("create-accounts")]
    public async Task CreateAccounts()
    {
        await CreateAccount(Guid.Parse("e1fed732-d485-4338-861d-58ebf16fb0d8"), Guid.Parse("bc1b25f7-53ba-45bd-b9ee-508429979b1d"), Guid.Parse("6325f8d1-94c0-492e-b891-689824c475fa"), "USD", 10_000);
        await CreateAccount(Guid.Parse("044b7cf9-64b9-4baf-ba3f-f616b22c3c70"), Guid.Parse("40c66ffe-91e2-4a44-bc76-7fd36f30cbf5"), Guid.Parse("e2b8a16b-bea0-440d-9c20-cf1964492342"), "EUR", 10_000);
        await CreateAccount(Guid.Parse("94c389ab-6abb-4e77-9077-3bf2765522ad"), Guid.Parse("419ff6d7-b11e-498c-811c-08a6656df6b5"), Guid.Parse("c244b99e-f1a7-430b-90dd-ded295df9da7"), "ETH", 1_000);
    }

    private async Task CreateAccount(Guid accountId, Guid fundsInAccountId, Guid transactionId, string currency, decimal amount)
    {
        var account = await _accountService.CreateAccount(new CreateAccountRequest { ExternalAccountId = accountId, Currency = currency, AccountType = AccountType.Virtual });

        try
        {
            await _currencyAccountRepository.Insert(new CurrencyAccountEntity { CurrencyAccountId = accountId, FundsAccountId = account.AccountId, Currency = currency });
        }
        catch
        {
        }

        var fundsInAccount = await _accountService.CreateAccount(new CreateAccountRequest { ExternalAccountId = fundsInAccountId, Currency = currency, AccountType = AccountType.FundsIn });

        try
        {
            await _currencyAccountRepository.Insert(new CurrencyAccountEntity { CurrencyAccountId = fundsInAccountId, FundsAccountId = fundsInAccount.AccountId, Currency = $"{currency}-FUNDS-IN" });
        }
        catch
        {
            
        }

        var allocation = await _allocationService.Allocate(new AllocateFundsRequest { AccountId = fundsInAccount.AccountId, Amount = amount, Currency = currency, ExternalTransactionId = transactionId });
        await _allocationService.SettleAllocation(new SettleAllocationRequest { AllocationId = allocation.AllocationId, DestinationAccountId = account.AccountId, ExternalTransactionId = transactionId });
    }
}