using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Exchange.Contract.FundExchange;
using Qwitter.Exchange.Contract.FundExchange.Models;
using Qwitter.Exchange.Service.CurrencyAccounts;
using Qwitter.Exchange.Service.FundExchange.Models;
using Qwitter.Exchange.Service.FundExchange.Repositories;
using Qwitter.Exchange.Service.Rate.Repositories;
using Qwitter.Funds.Contract.Allocations;
using Qwitter.Funds.Contract.Allocations.Models;

namespace Qwitter.Exchange.Service.FundExchange;

[ApiController]
[Route("fund-exchange")]
public class FundExchangeService : ControllerBase, IFundExchangeService
{
    private readonly IAllocationService _allocationService;
    private readonly IFxRateRepository _fxRateRepository;
    private readonly ICurrencyAccountService _currencyAccountService;
    private readonly IFundExchangeRepository _fundExchangeRepository;
    private readonly ILogger<FundExchangeService> _logger;

    public FundExchangeService(
        IAllocationService allocationService,
        IFxRateRepository fxRateRepository,
        ICurrencyAccountService currencyAccountService,
        IFundExchangeRepository fundExchangeRepository,
        ILogger<FundExchangeService> logger)
    {
        _allocationService = allocationService;
        _fxRateRepository = fxRateRepository;
        _currencyAccountService = currencyAccountService;
        _fundExchangeRepository = fundExchangeRepository;
        _logger = logger;
    }

    [HttpPost("convert")]
    public async Task<AllocationConversionResponse> ConvertAllocationCurrency(ConvertAllocationCurrencyRequest request)
    {
        var allocation = await _allocationService.GetAllocation(request.AllocationId);
        var rate = await _fxRateRepository.GetById(request.FxRateId);

        if (allocation.Currency != rate.SourceCurrency)
        {
            throw new BadRequestApiException($"Allocation currency {allocation.Currency} does not match FxRate currency {rate.SourceCurrency}");
        }

        if (!allocation.CorrelationId.HasValue)
        {
            throw new BadRequestApiException("Allocations must have a CorrelationId in order to be converted");
        }

        var destinationAmount = allocation.Amount * rate.Rate;

        var sourceCurrencyAccount = await _currencyAccountService.GetSourceCurrencyAccount(allocation.Currency);
        var destinationCurrencyAccount = await _currencyAccountService.GetDestinationCurrencyAccount(rate.DestinationCurrency);

        var transaction = new FundExchangeEntity
        {
            TransactionId = Guid.NewGuid(),
            SourceCurrency = rate.SourceCurrency,
            DestinationCurrency = rate.DestinationCurrency,
            FxRateId = rate.FxRateId,
            Rate = rate.Rate,
            SourceAmount = allocation.Amount,
            DestinationAmount = destinationAmount,
            SourceCurrencyAccountId = sourceCurrencyAccount.FundsAccountId,
            DestinationCurrencyAccountId = destinationCurrencyAccount.FundsAccountId,
            SourceAllocationId = allocation.AllocationId,
            CorrelationId = allocation.CorrelationId.Value,
            Status = FundExchangeStatus.Initiated,
        };

        await _fundExchangeRepository.Insert(transaction);

        await AllocateDestinationAmount(transaction);
        await SettleSourceAmount(transaction);

        return new AllocationConversionResponse
        {
            TransactionId = transaction.TransactionId,
            DestinationAllocationId = transaction.DestinationAllocationId!.Value
        };
    }

    private async Task AllocateDestinationAmount(FundExchangeEntity transaction)
    {
        try
        {
            var response = await _allocationService.Allocate(new AllocateFundsRequest
            {
                AccountId = transaction.DestinationCurrencyAccountId,
                ExternalTransactionId = transaction.TransactionId,
                CorrelationId = transaction.CorrelationId,
                Currency = transaction.DestinationCurrency,
                Amount = transaction.DestinationAmount
            });

            transaction.Status |= FundExchangeStatus.DestinationAllocated;
            transaction.DestinationAllocationId = response.AllocationId;

            await _fundExchangeRepository.Update(transaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to allocate destination funds for currency convertion");

            transaction.Status |= FundExchangeStatus.Failed;
            await _fundExchangeRepository.Update(transaction);

            throw;
        }
    }

    private async Task SettleSourceAmount(FundExchangeEntity transaction)
    {
        try
        {
            await _allocationService.SettleAllocation(new SettleAllocationRequest
            {
                AllocationId = transaction.SourceAllocationId,
                DestinationAccountId = transaction.DestinationCurrencyAccountId,
                ExternalTransactionId = transaction.TransactionId
            });

            transaction.Status |= FundExchangeStatus.SourceSettled;

            await _fundExchangeRepository.Update(transaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to settle source funds for currency convertion");

            transaction.Status |= FundExchangeStatus.Failed;
            await _fundExchangeRepository.Update(transaction);

            // TODO: Reverse destination allocation

            throw;
        }
    }
}