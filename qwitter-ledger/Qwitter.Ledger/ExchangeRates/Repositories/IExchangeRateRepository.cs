using Microsoft.EntityFrameworkCore;

namespace Qwitter.Ledger.ExchangeRates.Repositories;

public interface IExchangeRateRepository
{
    Task<decimal?> GetExchangeRate(string source, string destination);
}

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly AppDbContext _dbContext;

    public ExchangeRateRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<decimal?> GetExchangeRate(string source, string destination)
    {
        if (source == destination)
        {
            return 1;
        }

        var rate = await _dbContext.ExchangeRates.FirstOrDefaultAsync(p => p.Source == source && p.Destination == destination);

        return rate?.Rate;
    }
}