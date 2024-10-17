using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Exchange.Service.Rate.Models;

namespace Qwitter.Exchange.Service.Rate.Repositories;

public interface IFxRateRepository
{
    Task Insert(FxRateEntity entity);
    Task<FxRateEntity> GetLatestByCurrencyPair(string sourceCurrency, string destinationCurrency);
    Task<FxRateEntity> GetById(Guid fxRateId);
}

public class FxRateRepository : IFxRateRepository
{
    private readonly ServiceDbContext _dbContext;

    public FxRateRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(FxRateEntity entity)
    {
        await _dbContext.FxRates.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FxRateEntity> GetLatestByCurrencyPair(string sourceCurrency, string destinationCurrency)
    {
        var entity = await _dbContext.FxRates
            .Where(x => x.SourceCurrency == sourceCurrency && x.DestinationCurrency == destinationCurrency)
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync();

        if (entity == null)
        {
            throw new NotFoundApiException($"Exchange rate not found for {sourceCurrency}/{destinationCurrency}");
        }

        return entity;
    }

    public async Task<FxRateEntity> GetById(Guid fxRateId)
    {
        var entity = await _dbContext.FxRates.FindAsync(fxRateId);

        if (entity == null)
        {
            throw new NotFoundApiException($"Exchange rate not found for {fxRateId}");
        }

        return entity;
    }
}