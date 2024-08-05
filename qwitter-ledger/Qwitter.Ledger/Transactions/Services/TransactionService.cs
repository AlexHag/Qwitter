using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.Contract.Transactions.Events;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.FundAllocations.Models;
using Qwitter.Ledger.FundAllocations.Models.Enums;
using Qwitter.Ledger.FundAllocations.Repositories;
using Qwitter.Ledger.FundAllocations.Services;
using Qwitter.Ledger.Transactions.Models;
using Qwitter.Ledger.Transactions.Repositories;

namespace Qwitter.Ledger.Transactions.Services;

public interface ITransactionService
{
    Task<(FundAllocationEntity, BankAccountTransactionEntity)> AllocateBankAccountFunds(AllocateFundsRequest request);
    Task<(FundAllocationEntity, BankAccountTransactionEntity)> SettleBankAccountAllocation(SettleAllocationRequest request);

    Task<BankAccountTransactionEntity> TransferFunds(TransferFundsRequest request);
}

public class TransactionService : ITransactionService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IFundAllocationRepository _fundAllocationRepository;
    private readonly IBankAccountTransactionRepository _accountTransactionRepository;
    private readonly IAllocationCurrencyExchangeService _allocationCurrencyExchangeService;
    private readonly IEventProducer _eventProducer;

    public TransactionService(
        IBankAccountRepository bankAccountRepository,
        IFundAllocationRepository fundAllocationRepository,
        IBankAccountTransactionRepository accountTransactionRepository,
        IAllocationCurrencyExchangeService allocationCurrencyExchangeService,
        IEventProducer eventProducer)
    {
        _bankAccountRepository = bankAccountRepository;
        _fundAllocationRepository = fundAllocationRepository;
        _accountTransactionRepository = accountTransactionRepository;
        _allocationCurrencyExchangeService = allocationCurrencyExchangeService;
        _eventProducer = eventProducer;
    }

    public async Task<BankAccountTransactionEntity> TransferFunds(TransferFundsRequest request)
    {
        var (sourceAllocation, sourceTransaction) = await AllocateBankAccountFunds(new AllocateFundsRequest
        {
            BankAccountId = request.FromBankAcountId,
            Amount = request.Amount
        });

        await SettleBankAccountAllocation(new SettleAllocationRequest
        {
            BankAccountId = request.ToBankAccountId,
            FundAllocationId = sourceAllocation.Id
        });

        return sourceTransaction;
    }

    public async Task<(FundAllocationEntity, BankAccountTransactionEntity)> AllocateBankAccountFunds(AllocateFundsRequest request)
    {
        var account = await _bankAccountRepository.GetById(request.BankAccountId) ?? throw new NotFoundApiException("Account not found");

        if (!account.CanTransfer(request.Amount, out var reason))
        {
            throw new BadRequestApiException(reason);
        }

        var allocation = new FundAllocationEntity
        {
            Id = Guid.NewGuid(),
            SourceAmount = request.Amount,
            SourceCurrency = account.Currency,
            Status = FundAllocationStatus.Pending,
            SourceDomain = FundDomain.BankAccount,
            SourceId = account.Id
        };

        await _fundAllocationRepository.Insert(allocation);

        var transaction = new BankAccountTransactionEntity
        {
            Id = Guid.NewGuid(),
            BankAccountId = account.Id,
            AllocationId = allocation.Id,
            PreviousBalance = account.Balance,
            NewBalance = account.Balance - request.Amount,
            Amount = request.Amount
        };

        await _accountTransactionRepository.Insert(transaction);

        account.Balance -= request.Amount;
        await _bankAccountRepository.Update(account);

        if (account.Balance < 0)
        {
            await _eventProducer.Produce(new TransactionOverdraftEvent
            {
                UserId = account.UserId,
                AccountId = account.Id,
            });
        }

        return (allocation, transaction);
    }

    public async Task<(FundAllocationEntity, BankAccountTransactionEntity)> SettleBankAccountAllocation(SettleAllocationRequest request)
    {
        var allocation = await _fundAllocationRepository.GetById(request.FundAllocationId) ?? throw new NotFoundApiException("Allocation not found");

        if (allocation.Status != FundAllocationStatus.Pending)
        {
            throw new BadRequestApiException($"Cannot settle allocation in status: {allocation.Status}");
        }

        var account = await _bankAccountRepository.GetById(request.BankAccountId) ?? throw new NotFoundApiException("Account not found");

        if (!account.IsActive(out var reason))
        {
            throw new BadRequestApiException(reason);
        }

        if (allocation.SourceCurrency != account.Currency)
        {
            allocation = await _allocationCurrencyExchangeService.ConvertAllocationCurrency(allocation.Id, account.Currency);
        }
        else
        {
            allocation.DestinationAmount = allocation.SourceAmount;
            allocation.DestinationCurrency = allocation.SourceCurrency;
            allocation.ExchangeRate = 1;
        }

        allocation.Fee = 0;
        // TODO: Validate destination
        allocation.DestinationDomain = FundDomain.BankAccount;
        allocation.DestinationId = account.Id;
        allocation.Status = FundAllocationStatus.Settled;
        await _fundAllocationRepository.Update(allocation);

        var transaction = new BankAccountTransactionEntity
        {
            Id = Guid.NewGuid(),
            BankAccountId = account.Id,
            AllocationId = allocation.Id,
            PreviousBalance = account.Balance,
            NewBalance = account.Balance + allocation.DestinationAmount.Value,
            Amount = allocation.DestinationAmount.Value
        };

        await _accountTransactionRepository.Insert(transaction);

        account.Balance += allocation.DestinationAmount.Value;
        await _bankAccountRepository.Update(account);

        return (allocation, transaction);
    }
}