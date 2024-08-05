using Qwitter.Core.Application.Kafka;
using Qwitter.Ledger.Contract.FundAllocations.Models;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.FundAllocations.Models;
using Qwitter.Ledger.FundAllocations.Models.Enums;
using Qwitter.Ledger.FundAllocations.Repositories;

namespace Qwitter.Ledger.FundAllocations.Services;

public interface IFundAllocationService
{
    Task Create(CreateFundAllocationRequest request);
}

public class FundAllocationService : IFundAllocationService
{
    private readonly IFundAllocationRepository _fundAllocationRepository;
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly IEventProducer _eventProducer;

    public FundAllocationService(
        IFundAllocationRepository fundAllocationRepository,
        IExchangeRateRepository exchangeRateRepository,
        IEventProducer eventProducer)
    {
        _fundAllocationRepository = fundAllocationRepository;
        _exchangeRateRepository = exchangeRateRepository;
        _eventProducer = eventProducer;
    }

    public async Task Create(CreateFundAllocationRequest request)
    {
        var allocation = new FundAllocationEntity
        {
            Id = Guid.NewGuid(),
            SourceId = request.SourceId,
            SourceDomain = request.SourceTopic,
            SourceAmount = request.Amount,
            SourceCurrency = request.Currency,

            DestinationId = request.DestinationId,
            DestinationDomain = request.DestinationTopic,

            Status = FundAllocationStatus.Pending
        };

        await _fundAllocationRepository.Insert(allocation);
    }

    // Validate destination
    // Find destination
    // Validate exchange rate

    // Find source if allocation fails

    // Decide on fees



    public async Task SettleAllocation(Guid allocationId)
    {
        var allocation = await _fundAllocationRepository.GetById(allocationId);

        if (allocation is null || allocation.Status != FundAllocationStatus.Pending)
        {
            throw new Exception("Allocation not found or not in pending state");
        }
    }

    private void Validate(FundAllocationEntity entity)
    {
        if (entity.Status != FundAllocationStatus.Pending)
        {
            throw new Exception("Allocation is not in pending state");
        }

        if (entity.DestinationId is null || entity.DestinationDomain is null)
        {
            throw new Exception("Destination is required for allocation");
        }

        if (entity.SourceAmount <= 0)
        {
            throw new Exception("Source amount must be greater than 0");
        }
    }

    private async Task ValidateExchangeRate(FundAllocationEntity entity)
    {
        var rate = await _exchangeRateRepository.GetExchangeRate(entity.SourceCurrency, entity.DestinationCurrency);
    }
}