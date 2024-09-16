using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Service.CurrencyExchange.Models;

namespace Qwitter.Funds.Service.CurrencyExchange.Repositories;

public interface ICurrencyAccountRepository
{
    Task<CurrencyAccountEntity> GetByCurrency(string currency);
    Task Update(CurrencyAccountEntity entity);
}

public class CurrencyAccountRepository : ICurrencyAccountRepository
{
    private readonly ServiceDbContext _dbContext;

    public CurrencyAccountRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CurrencyAccountEntity> GetByCurrency(string currency)
    {
        var entity = await _dbContext.CurrencyAccounts.FirstOrDefaultAsync(x => x.Currency == currency);

        if (entity == null)
        {
            throw new NotFoundApiException($"Currency account with currency {currency} not found");
        }

        return entity;
    }

    public async Task Update(CurrencyAccountEntity entity)
    {
        _dbContext.CurrencyAccounts.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}