
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Service.ExchangeRate.Models;

namespace Qwitter.Funds.Service.ExchangeRate.Repositories;

public interface IExchangeRateRepository
{
    Task Insert(ExchangeRateEntity entity);
    Task<ExchangeRateEntity> GetLatestByCurrencyPair(string sourceCurrency, string destinationCurrency);
}

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly ServiceDbContext _dbContext;

    public ExchangeRateRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(ExchangeRateEntity entity)
    {
        await _dbContext.ExchangeRates.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ExchangeRateEntity> GetLatestByCurrencyPair(string sourceCurrency, string destinationCurrency)
    {
        var entity = await _dbContext.ExchangeRates
            .Where(x => x.SourceCurrency == sourceCurrency && x.DestinationCurrency == destinationCurrency)
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new NotFoundApiException($"Exchange rate not found for {sourceCurrency}/{destinationCurrency}");
        }

        return entity;
    }
}