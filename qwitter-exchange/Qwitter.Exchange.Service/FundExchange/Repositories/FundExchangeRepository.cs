using Qwitter.Exchange.Service.FundExchange.Models;

namespace Qwitter.Exchange.Service.FundExchange.Repositories;

public interface IFundExchangeRepository
{
    Task Insert(FundExchangeEntity entity);
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
        await _dbContext.FundExchanges.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
}