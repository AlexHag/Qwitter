using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Service.Accounts.Models;

namespace Qwitter.Funds.Service.Accounts.Repositories;

public interface IAccountRepository
{
    Task Insert(AccountEntity entity);
    Task Update(AccountEntity entity);
    Task<AccountEntity> GetById(Guid id);
    Task<AccountEntity?> TryGetByOwnerReferenceIdAndCurrency(Guid ownerReferenceId, string currency);
}

public class AccountRepository : IAccountRepository
{
    private readonly ServiceDbContext _dbContext;

    public AccountRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(AccountEntity entity)
    {
        await _dbContext.Accounts.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(AccountEntity entity)
    {
        _dbContext.Accounts.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AccountEntity> GetById(Guid id)
    {
        var entity = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.AccountId == id);
        
        if (entity == null)
        {
            throw new NotFoundApiException($"Account {id} not found");
        }

        return entity;
    }

    public async Task<AccountEntity?> TryGetByOwnerReferenceIdAndCurrency(Guid ownerReferenceId, string currency)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.OwnerReferenceId == ownerReferenceId && x.Currency == currency);
    }
}