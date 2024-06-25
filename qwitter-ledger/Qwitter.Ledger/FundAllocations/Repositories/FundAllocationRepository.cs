using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.FundAllocations.Models;

namespace Qwitter.Ledger.FundAllocations.Repositories;

public interface IFundAllocationRepository
{
    Task Insert(FundAllocationEntity entity);
    Task Update(FundAllocationEntity entity);
    Task<FundAllocationEntity?> GetById(Guid id);
}

public class FundAllocationRepository : IFundAllocationRepository
{
    private readonly AppDbContext _dbContext;

    public FundAllocationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(FundAllocationEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbContext.FundAllocations.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(FundAllocationEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbContext.FundAllocations.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FundAllocationEntity?> GetById(Guid id)
        => await _dbContext.FundAllocations.FirstOrDefaultAsync(x => x.Id == id);
}