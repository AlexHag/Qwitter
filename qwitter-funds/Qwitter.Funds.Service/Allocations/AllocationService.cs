using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Accounts.Enums;
using Qwitter.Funds.Contract.Allocations;
using Qwitter.Funds.Contract.Allocations.Enums;
using Qwitter.Funds.Contract.Allocations.Models;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.Allocations.Repositories;
using Qwitter.Funds.Service.Clients.Repositories;

namespace Qwitter.Funds.Service.Allocations;

[ApiController]
[Route("allocations")]
[Authorize(AuthenticationSchemes = "mTLS")]
public class AllocationService : ControllerBase, IAllocationService
{
    private readonly IMapper _mapper;
    private readonly IAllocationRepository _allocationRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AllocationService> _logger;

    public AllocationService(
        IMapper mapper,
        IAllocationRepository allocationRepository,
        IClientRepository clientRepository,
        IAccountRepository accountRepository,
        ILogger<AllocationService> logger)
    {
        _mapper = mapper;
        _allocationRepository = allocationRepository;
        _clientRepository = clientRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    [HttpPost("allocate")]
    public async Task<AllocationResponse> Allocate(AllocateFundsRequest request)
    {
        var thumbprint = HttpContext.Connection.ClientCertificate!.Thumbprint;

        var client = await _clientRepository.GetByThumbprint(thumbprint);
        var account = await _accountRepository.GetById(request.AccountId);

        if (account.ClientId != client.ClientId)
        {
            throw new ForbiddenApiException($"Account {account.AccountId} does not belong to client {client.ClientName}");
        }

        if (account.AccountType == AccountType.FundsOut)
        {
            throw new ForbiddenApiException($"Not allowed to allocate funds from a funds out account");
        }

        var duplicateAllocation = await _allocationRepository.TryGetByExternalSourceTransactionId(request.ExternalTransactionId);

        if (duplicateAllocation != null)
        {
            return _mapper.Map<AllocationResponse>(duplicateAllocation);
        }

        if (account.AccountType == AccountType.Virtual && account.Balance < request.Amount)
        {
            throw new BadRequestApiException($"Insufficient funds in account {account.AccountId}");
        }

        var allocation = new AllocationEntity
        {
            AllocationId = Guid.NewGuid(),
            SourceAccountId = request.AccountId,
            CorrelationId = request.CorrelationId,
            ExternalSourceTransactionId = request.ExternalTransactionId,
            Currency = account.Currency,
            Amount = request.Amount,
            Status = AllocationStatus.Allocated,
            AllocatedAt = DateTime.UtcNow
        };

        account.Balance -= request.Amount;

        Console.WriteLine(JsonConvert.SerializeObject(allocation, Formatting.Indented));

        await _allocationRepository.Insert(allocation);
        await _accountRepository.Update(account);

        // TODO: Publish event

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
        var thumbprint = HttpContext.Connection.ClientCertificate!.Thumbprint;

        var allocation = await _allocationRepository.GetById(request.AllocationId);

        if (allocation.Status != AllocationStatus.Allocated)
        {
            _logger.LogError("Allocation {AllocationId} in status {Status} cannot be settled", allocation.AllocationId, allocation.Status);
            throw new BadRequestApiException($"Allocation {allocation.AllocationId} in status {allocation.Status} cannot be settled");
        }

        var client = await _clientRepository.GetByThumbprint(thumbprint);
        var account = await _accountRepository.GetById(request.DestinationAccountId);

        if (account.ClientId != client.ClientId)
        {
            throw new ForbiddenApiException($"Account {account.AccountId} does not belong to client {client.ClientName}");
        }

        if (account.Currency != allocation.Currency)
        {
            throw new BadRequestApiException($"Account {account.AccountId} currency {account.Currency} does not match allocation currency {allocation.Currency}");
        }

        if (account.AccountType == AccountType.FundsIn)
        {
            throw new ForbiddenApiException("Not allowed to settle funds into a FundsIn account");
        }

        account.Balance += allocation.Amount;

        allocation.DestinationAccountId = account.AccountId;
        allocation.ExternalDestinationTransactionId = request.ExternalTransactionId;
        allocation.Status = AllocationStatus.Settled;
        allocation.SettledAt = DateTime.UtcNow;

        await _accountRepository.Update(account);
        await _allocationRepository.Update(allocation);

        // TODO: Publish event

        return _mapper.Map<AllocationResponse>(allocation);
    }
}
