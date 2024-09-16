
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Service.Allocations.Models;

namespace Qwitter.Funds.Service.Allocations.Repositories;

public interface IAllocationRepository
{
    Task Insert(AllocationEntity entity);
    Task Update(AllocationEntity entity);
    Task<AllocationEntity> GetById(Guid allocationId);
}

public class AllocationRepository : IAllocationRepository
{
    private readonly ServiceDbContext _dbContext;

    public AllocationRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(AllocationEntity entity)
    {
        await _dbContext.Allocations.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(AllocationEntity entity)
    {
        _dbContext.Allocations.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AllocationEntity> GetById(Guid allocationId)
    {
        var entity = await _dbContext.Allocations.FirstOrDefaultAsync(a => a.AllocationId == allocationId);

        if (entity == null)
        {
            throw new NotFoundApiException($"Allocation {allocationId} not found");
        }

        return entity;
    }
}