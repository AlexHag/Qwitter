
using Qwitter.Funds.Service.CurrencyExchange.Models;

namespace Qwitter.Funds.Service.CurrencyExchange.Repositories;

public interface ICurrencyExchangeRepository
{
    Task Insert(CurrencyExchangeEntity entity);
}

public class CurrencyExchangeRepository : ICurrencyExchangeRepository
{
    private readonly ServiceDbContext _dbContext;

    public CurrencyExchangeRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(CurrencyExchangeEntity entity)
    {
        await _dbContext.CurrencyExchanges.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
}