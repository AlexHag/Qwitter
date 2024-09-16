using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Allocations.Models;

namespace Qwitter.Funds.Contract.Allocations;

[ApiHost(Host.Name, "funds")]
public interface IAllocationService
{
    [HttpPost("allocate")]
    Task<AllocationResponse> Allocate(AllocateFundsRequest request);

    [HttpPost("settle")]
    Task<AllocationResponse> SettleAllocation(SettleAllocationRequest request);

    [HttpPost("convert")]
    Task<AllocationResponse> Convert(ConvertAllocationRequest request);
}