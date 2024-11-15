using Qwitter.Exchange.Service.FundExchange.Models;

namespace Qwitter.Exchange.Service.FundExchange.Repositories;

public interface IFundExchangeRepository
{
    Task Insert(FundExchangeEntity entity);
    Task Update(FundExchangeEntity entity);
}

public class FundExchangeRepository : IFundExchangeRepository
{
    private readonly ServiceDbContext _dbContext;

    public FundExchangeRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(FundExchangeEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbContext.FundExchanges.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task Update(FundExchangeEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbContext.FundExchanges.Update(entity);
        return _dbContext.SaveChangesAsync();
    }
}