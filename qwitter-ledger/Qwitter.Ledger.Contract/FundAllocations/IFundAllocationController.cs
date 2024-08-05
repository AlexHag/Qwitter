using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.FundAllocations.Models;

namespace Qwitter.Ledger.Contract.FundAllocations;

[ApiHost(Host.Port, "fund-allocation")]
public interface IFundAllocationController
{
    [HttpPost("create")]
    Task Create(CreateFundAllocationRequest request);
}