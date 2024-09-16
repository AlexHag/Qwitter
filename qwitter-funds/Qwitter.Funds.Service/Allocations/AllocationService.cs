using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Allocations;
using Qwitter.Funds.Contract.Allocations.Enums;
using Qwitter.Funds.Contract.Allocations.Models;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.Allocations.Repositories;
using Qwitter.Funds.Service.CurrencyExchange;
using Qwitter.Funds.Service.CurrencyExchange.Models;

namespace Qwitter.Funds.Service.Allocations;

[ApiController]
[Route("funds")]
public class AllocationService : ControllerBase, IAllocationService
{
    private readonly IMapper _mapper;
    private readonly IAllocationRepository _allocationRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchangeActions _currencyExchangeActions;

    public AllocationService(
        IMapper mapper,
        IAllocationRepository allocationRepository,
        IAccountRepository accountRepository,
        ICurrencyExchangeActions currencyExchangeActions)
    {
        _mapper = mapper;
        _allocationRepository = allocationRepository;
        _accountRepository = accountRepository;
        _currencyExchangeActions = currencyExchangeActions;
    }

    [HttpPost("allocate")]
    public async Task<AllocationResponse> Allocate(AllocateFundsRequest request)
    {
        var account = await _accountRepository.GetById(request.AccountId);

        var allocation = new AllocationEntity
        {
            AllocationId = Guid.NewGuid(),
            Currency = account.Currency,
            Amount = request.Amount,
            SourceAccountId = request.AccountId,
            Status = AllocationStatus.Allocated
        };

        account.Balance -= request.Amount;

        await _allocationRepository.Insert(allocation);
        await _accountRepository.Update(account);

        // TODO: Publish event

        return _mapper.Map<AllocationResponse>(allocation);
    }

    [HttpPost("convert")]
    public async Task<AllocationResponse> Convert(ConvertAllocationRequest request)
    {
        var allocation = await _allocationRepository.GetById(request.AllocationId);

        if (allocation.Status != AllocationStatus.Allocated)
        {
            throw new BadRequestApiException($"Allocation {allocation.AllocationId} in status {allocation.Status} cannot be converted");
        }

        var exchange = await _currencyExchangeActions.Exchange(new ExchangeCurrencyCommand
        {
            SourceCurrency = allocation.Currency,
            DestinationCurrency = request.Currency,
            Amount = allocation.Amount
        });

        var convertedAllocation = new AllocationEntity
        {
            AllocationId = Guid.NewGuid(),
            Currency = exchange.DestinationCurrency,
            Amount = exchange.DestinationAmount,
            Status = AllocationStatus.Allocated,
            SourceAccountId = allocation.SourceAccountId,
            CurrencyExchangeId = exchange.CurrencyExchangeId
        };

        allocation.Status = AllocationStatus.Converted;
        allocation.ConvertedIntoAllocationId = convertedAllocation.AllocationId;
        allocation.CurrencyExchangeId = exchange.CurrencyExchangeId;

        await _allocationRepository.Insert(convertedAllocation);
        await _allocationRepository.Update(allocation);

        // TODO: Publish event maybe...

        return _mapper.Map<AllocationResponse>(convertedAllocation);
    }

    [HttpPost("settle")]
    public async Task<AllocationResponse> SettleAllocation(SettleAllocationRequest request)
    {
        var account = await _accountRepository.GetById(request.AccountId);
        var allocation = await _allocationRepository.GetById(request.AllocationId);

        if (allocation.Status != AllocationStatus.Allocated)
        {
            throw new BadRequestApiException($"Allocation {allocation.AllocationId} in status {allocation.Status} cannot be settled");
        }

        if (allocation.Currency != account.Currency)
        {
            throw new BadRequestApiException($"Allocation {allocation.AllocationId} currency {allocation.Currency} does not match account currency {account.Currency}. Allocation must be converted first");
        }

        account.Balance += allocation.Amount;
        allocation.Status = AllocationStatus.Settled;
        allocation.DestinationAccountId = request.AccountId;

        await _accountRepository.Update(account);
        await _allocationRepository.Update(allocation);

        // TODO: Publish event

        return _mapper.Map<AllocationResponse>(allocation);
    }
}
