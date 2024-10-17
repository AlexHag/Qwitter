using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Allocations;
using Qwitter.Funds.Contract.Allocations.Enums;
using Qwitter.Funds.Contract.Allocations.Models;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.Allocations.Repositories;

namespace Qwitter.Funds.Service.Allocations;

[ApiController]
[Route("allocations")]
public class AllocationService : ControllerBase, IAllocationService
{
    private readonly IMapper _mapper;
    private readonly IAllocationRepository _allocationRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AllocationService> _logger;

    public AllocationService(
        IMapper mapper,
        IAllocationRepository allocationRepository,
        IAccountRepository accountRepository,
        ILogger<AllocationService> logger)
    {
        _mapper = mapper;
        _allocationRepository = allocationRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    [HttpPost("allocate")]
    public async Task<AllocationResponse> Allocate(AllocateFundsRequest request)
    {
        var account = await _accountRepository.GetById(request.AccountId);

        var duplicateAllocation = await _allocationRepository.TryGetByTransactionId(request.TransactionId);

        if (duplicateAllocation != null)
        {
            _logger.LogWarning("Duplicate allocation request {TransactionId} for account {AccountId}", request.TransactionId, request.AccountId);
            return _mapper.Map<AllocationResponse>(duplicateAllocation);
        }

        var allocation = new AllocationEntity
        {
            AllocationId = Guid.NewGuid(),
            AccountId = request.AccountId,
            TransactionId = request.TransactionId,
            Currency = account.Currency,
            Amount = request.Amount,
            Status = AllocationStatus.Allocated
        };

        account.AvailableBalance -= request.Amount;

        await _allocationRepository.Insert(allocation);
        await _accountRepository.Update(account);

        return _mapper.Map<AllocationResponse>(allocation);
    }

    [HttpGet("{allocationId}")]
    public async Task<AllocationResponse> GetAllocation(Guid allocationId)
    {
        var allocation = await _allocationRepository.GetById(allocationId);
        return _mapper.Map<AllocationResponse>(allocation);
    }

    [HttpPost("settle")]
    public async Task<AllocationResponse> SettleAllocation(SettleAllocationRequest request)
    {
        var allocation = await _allocationRepository.GetById(request.AllocationId);
        var sourceAccount = await _accountRepository.GetById(allocation.AccountId);
        var destinationAccount = await _accountRepository.GetById(request.DestinationAccountId);

        if (allocation.Status != AllocationStatus.Allocated)
        {
            _logger.LogError("Allocation {AllocationId} in status {Status} cannot be settled", allocation.AllocationId, allocation.Status);
            throw new BadRequestApiException($"Allocation {allocation.AllocationId} in status {allocation.Status} cannot be settled");
        }

        if (allocation.Currency != destinationAccount.Currency)
        {
            _logger.LogError("Allocation {AllocationId} currency {Currency} does not match account currency {AccountCurrency}", allocation.AllocationId, allocation.Currency, destinationAccount.Currency);
            throw new BadRequestApiException($"Allocation {allocation.AllocationId} currency {allocation.Currency} does not match account currency {destinationAccount.Currency}. Allocation must be converted first");
        }

        sourceAccount.TotalBalance -= allocation.Amount;
        destinationAccount.AvailableBalance += allocation.Amount;
        destinationAccount.TotalBalance += allocation.Amount;

        allocation.Status = AllocationStatus.Settled;
        allocation.DestinationAccountId = destinationAccount.AccountId;

        await _accountRepository.Update(destinationAccount);
        await _allocationRepository.Update(allocation);

        // TODO: Publish event

        return _mapper.Map<AllocationResponse>(allocation);
    }
}
