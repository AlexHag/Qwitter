using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Exchange.Contract.FundExchange.Models;

namespace Qwitter.Exchange.Contract.FundExchange;

[ApiHost(Host.Name, "fund-exchange")]
public interface IFundExchangeService
{
    [HttpPost("convert")]
    Task<AllocationConversionResponse> ConvertAllocationCurrency(ConvertAllocationCurrencyRequest request);
}