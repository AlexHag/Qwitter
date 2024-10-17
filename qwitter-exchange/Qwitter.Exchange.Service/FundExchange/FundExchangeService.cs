using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Exchange.Contract.FundExchange;
using Qwitter.Exchange.Contract.FundExchange.Models;
using Qwitter.Exchange.Service.CurrencyAccounts.Repositories;
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
    private readonly ICurrencyAccountRepository _currencyAccountRepository;
    private readonly IFundExchangeRepository _fundExchangeRepository;
    private readonly ILogger<FundExchangeService> _logger;

    public FundExchangeService(
        IAllocationService allocationService,
        IFxRateRepository fxRateRepository,
        ICurrencyAccountRepository currencyAccountRepository,
        IFundExchangeRepository fundExchangeRepository,
        ILogger<FundExchangeService> logger)
    {
        _allocationService = allocationService;
        _fxRateRepository = fxRateRepository;
        _currencyAccountRepository = currencyAccountRepository;
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
            _logger.LogError("Allocation currency {allocationCurrency} does not match FxRate currency {fxRateCurrency}", allocation.Currency, rate.SourceCurrency);
            throw new BadRequestApiException($"Allocation currency {allocation.Currency} does not match FxRate currency {rate.SourceCurrency}");
        }

        var sourceAccount = await _currencyAccountRepository.GetByCurrency(allocation.Currency);
        var destinationAmount = allocation.Amount * rate.Rate;

        if (sourceAccount.Balance < allocation.Amount)
        {
            _logger.LogError("Insufficient funds in source account {sourceAccountId}", sourceAccount.CurrencyAccountId);
            throw new BadRequestApiException($"Insufficient funds in source account {sourceAccount.CurrencyAccountId}");
        }

        var transactionId = Guid.NewGuid();
        var destinationAccount = await _currencyAccountRepository.GetByCurrency(rate.DestinationCurrency);

        var destinationAllocation = await _allocationService.Allocate(new AllocateFundsRequest
        {
            AccountId = destinationAccount.FundsAccountId,
            TransactionId = transactionId,
            Currency = rate.DestinationCurrency,
            Amount = destinationAmount
        });

        // TODO: Try catch and reverse allocation if settlement fails
        _ = await _allocationService.SettleAllocation(new SettleAllocationRequest
        {
            AllocationId = allocation.AllocationId,
            DestinationAccountId = sourceAccount.FundsAccountId
        });

        var entity = new FundExchangeEntity
        {
            TransactionId = transactionId,
            SourceCurrency = rate.SourceCurrency,
            DestinationCurrency = rate.DestinationCurrency,
            FxRateId = rate.FxRateId,
            Rate = rate.Rate,
            SourceAmount = allocation.Amount,
            DestinationAmount = destinationAmount,
            SourceAllocationId = allocation.AllocationId,
            DestinationAllocationId = destinationAllocation.AllocationId
        };

        sourceAccount.Balance += allocation.Amount;
        destinationAccount.Balance -= destinationAmount;

        await _currencyAccountRepository.Update(sourceAccount);
        await _currencyAccountRepository.Update(destinationAccount);
        await _fundExchangeRepository.Insert(entity);

        var response = new AllocationConversionResponse
        {
            TransactionId = transactionId,
            DestinationAllocationId = destinationAllocation.AllocationId,
        };

        return response;
    }
}