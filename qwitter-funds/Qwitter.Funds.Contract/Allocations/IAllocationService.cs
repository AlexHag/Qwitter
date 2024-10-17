using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Allocations.Models;

namespace Qwitter.Funds.Contract.Allocations;

[ApiHost(Host.Name, "allocations")]
public interface IAllocationService
{
    [HttpGet("{allocationId}")]
    Task<AllocationResponse> GetAllocation(Guid allocationId);

    [HttpPost("allocate")]
    Task<AllocationResponse> Allocate(AllocateFundsRequest request);

    [HttpPost("settle")]
    Task<AllocationResponse> SettleAllocation(SettleAllocationRequest request);
}