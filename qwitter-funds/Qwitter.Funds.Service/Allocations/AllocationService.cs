using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Funds.Contract.Accounts.Enums;
using Qwitter.Funds.Contract.Allocations;
using Qwitter.Funds.Contract.Allocations.Enums;
using Qwitter.Funds.Contract.Allocations.Models;
using Qwitter.Funds.Contract.Events;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.Allocations.Repositories;
using Qwitter.Funds.Service.Clients.Repositories;
using Qwitter.Funds.Service.Transactions.Handler;

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
    private readonly ITransactionHandler _transactionHandler;
    private readonly IEventProducer _eventProducer;
    private readonly ILogger<AllocationService> _logger;

    public AllocationService(
        IMapper mapper,
        IAllocationRepository allocationRepository,
        IClientRepository clientRepository,
        IAccountRepository accountRepository,
        ITransactionHandler transactionHandler,
        IEventProducer eventProducer,
        ILogger<AllocationService> logger)
    {
        _mapper = mapper;
        _allocationRepository = allocationRepository;
        _clientRepository = clientRepository;
        _accountRepository = accountRepository;
        _transactionHandler = transactionHandler;
        _eventProducer = eventProducer;
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
            ExternalSourceTransactionId = request.ExternalTransactionId,
            Currency = account.Currency,
            Amount = request.Amount,
            Status = AllocationStatus.Allocated,
            AllocatedAt = DateTime.UtcNow
        };

        await _allocationRepository.Insert(allocation);
        await _transactionHandler.DebitFunds(new() { AccountId = request.AccountId, AllocationId = allocation.AllocationId, Currency = account.Currency, Amount = request.Amount });

        await _eventProducer.Produce(new FundsAllocatedEvent { AllocationId = allocation.AllocationId}, client.ClientName);

        return _mapper.Map<AllocationResponse>(allocation);
    }

    [HttpGet("{allocationId}")]
    public async Task<AllocationResponse> GetAllocation(Guid allocationId)
    {
        var allocation = await _allocationRepository.GetById(allocationId);
        return _mapper.Map<AllocationResponse>(allocation);
    }

    [HttpPost("settle")]
    public async Task<AllocationResponse> Settle(SettleAllocationRequest request)
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

        allocation.DestinationAccountId = account.AccountId;
        allocation.ExternalDestinationTransactionId = request.ExternalTransactionId;
        allocation.Status = AllocationStatus.Settled;
        allocation.SettledAt = DateTime.UtcNow;

        await _allocationRepository.Update(allocation);

        await _transactionHandler.CreditFunds(new() { AccountId = request.DestinationAccountId, AllocationId = allocation.AllocationId, Currency = account.Currency, Amount = allocation.Amount });

        await _eventProducer.Produce(new AllocationSettledEvent { AllocationId = allocation.AllocationId }, client.ClientName);

        return _mapper.Map<AllocationResponse>(allocation);
    }
}
