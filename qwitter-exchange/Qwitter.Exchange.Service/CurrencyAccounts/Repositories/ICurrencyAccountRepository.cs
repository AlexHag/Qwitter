
using Microsoft.EntityFrameworkCore;
using Qwitter.Exchange.Service.CurrencyAccounts.Models;

namespace Qwitter.Exchange.Service.CurrencyAccounts.Repositories;

public interface ICurrencyAccountRepository
{
    Task<CurrencyAccountEntity> GetByCurrency(string currency);
    Task Update(CurrencyAccountEntity entity);
    Task Insert(CurrencyAccountEntity entity);
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
        var account = await _dbContext.CurrencyAccounts.FirstOrDefaultAsync(x => x.Currency == currency);

        if (account == null)
        {
            throw new Exception($"Account with currency {currency} not found");
        }

        return account;
    }

    public async Task Insert(CurrencyAccountEntity entity)
    {
        await _dbContext.CurrencyAccounts.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(CurrencyAccountEntity entity)
    {
        _dbContext.CurrencyAccounts.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}