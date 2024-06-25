using Qwitter.Core.Application.Exceptions;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.FundAllocations.Models;
using Qwitter.Ledger.FundAllocations.Models.Enums;
using Qwitter.Ledger.FundAllocations.Repositories;

namespace Qwitter.Ledger.Transactions.Services;

public interface IAllocationCurrencyExchangeService
{
    Task<FundAllocationEntity> ConvertAllocationCurrency(Guid allocationId, string currency);
}

public class AllocationCurrencyExchangeService : IAllocationCurrencyExchangeService
{
    private readonly IFundAllocationRepository _fundAllocationRepository;
    private readonly ISystemTransactionService _systemTransactionService;
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public AllocationCurrencyExchangeService(
        IFundAllocationRepository fundAllocationRepository,
        ISystemTransactionService systemTransactionService,
        IExchangeRateRepository exchangeRateRepository)
    {
        _fundAllocationRepository = fundAllocationRepository;
        _systemTransactionService = systemTransactionService;
        _exchangeRateRepository = exchangeRateRepository;
    }

    public async Task<FundAllocationEntity> ConvertAllocationCurrency(Guid allocationId, string currency)
    {
        var allocation = await _fundAllocationRepository.GetById(allocationId) ?? throw new NotFoundApiException("Allocation not found");

        if (allocation.Status != FundAllocationStatus.Hold)
        {
            throw new BadRequestApiException($"Cannot settle allocation in status: {allocation.Status}");
        }

        if (allocation.SourceCurrency == currency)
        {
            allocation.DestinationAmount = allocation.SourceAmount;
            allocation.DestinationCurrency = allocation.SourceCurrency;
            allocation.ExchangeRate = 1;
            await _fundAllocationRepository.Update(allocation);
            return allocation;
        }

        var rate = await _exchangeRateRepository.GetExchangeRate(allocation.SourceCurrency, currency) ?? throw new NotFoundApiException($"Unsupported currency exchange: {allocation.SourceCurrency} - {currency}");
        var destinationAmount = allocation.SourceAmount * rate;

        allocation.DestinationAmount = destinationAmount;
        allocation.DestinationCurrency = currency;
        allocation.ExchangeRate = rate;

        await _systemTransactionService.CreditSystemCurrency(allocation.SourceCurrency, allocation.SourceAmount);
        await _systemTransactionService.DebitSystemCurrency(currency, destinationAmount);

        await _fundAllocationRepository.Update(allocation);

        return allocation;
    }
}