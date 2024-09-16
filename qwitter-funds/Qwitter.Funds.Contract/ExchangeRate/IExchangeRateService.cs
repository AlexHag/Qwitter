using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.ExchangeRate.Models;

namespace Qwitter.Funds.Contract.ExchangeRate;

[ApiHost(Host.Name, "exchange-rate")]
public interface IExchangeRateService
{
    [HttpGet("{sourceCurrency}/{destinationCurrency}")]
    Task<ExchangeRateModel> GetRate(string sourceCurrency, string destinationCurrency);

    [HttpPut("update")]
    Task<ExchangeRateModel> UpdateRate(ExchangeRateModel request);
}