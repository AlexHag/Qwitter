using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Exchange.Contract.FxRate.Models;

namespace Qwitter.Exchange.Contract.FxRate;

[ApiHost(Host.Name, "exchange-rate")]
public interface IFxRateService
{
    [HttpGet("{sourceCurrency}/{destinationCurrency}")]
    Task<FxRateResponse> GetFxRate(string sourceCurrency, string destinationCurrency);

    [HttpPut("update")]
    Task<FxRateResponse> UpdateFxRate(UpdateFxRateRequest request);
}