
using Microsoft.AspNetCore.Mvc;
using Qwitter.Funds.Contract.ExchangeRate;
using Qwitter.Funds.Contract.ExchangeRate.Models;
using Qwitter.Funds.Service.ExchangeRate.Models;
using Qwitter.Funds.Service.ExchangeRate.Repositories;

namespace Qwitter.Funds.Service.ExchangeRate;

[ApiController]
[Route("exchange-rate")]
public class ExchangeRateService : ControllerBase, IExchangeRateService
{
    private readonly IExchangeRateRepository _exchangeRateRepository;

    public ExchangeRateService(IExchangeRateRepository exchangeRateRepository)
    {
        _exchangeRateRepository = exchangeRateRepository;
    }

    [HttpGet("{sourceCurrency}/{destinationCurrency}")]
    public async Task<ExchangeRateModel> GetRate(string sourceCurrency, string destinationCurrency)
    {
        var rate = await _exchangeRateRepository.GetLatestByCurrencyPair(sourceCurrency, destinationCurrency);

        return new ExchangeRateModel
        {
            SourceCurrency = rate.SourceCurrency,
            DestinationCurrency = rate.DestinationCurrency,
            Rate = rate.Rate
        };
    }

    [HttpPut("update")]
    public async Task<ExchangeRateModel> UpdateRate(ExchangeRateModel request)
    {
        var rate = new ExchangeRateEntity
        {
            SourceCurrency = request.SourceCurrency,
            DestinationCurrency = request.DestinationCurrency,
            Rate = request.Rate,
            Created = DateTime.UtcNow
        };

        await _exchangeRateRepository.Insert(rate);

        return new ExchangeRateModel
        {
            SourceCurrency = rate.SourceCurrency,
            DestinationCurrency = rate.DestinationCurrency,
            Rate = rate.Rate
        };
    }
}