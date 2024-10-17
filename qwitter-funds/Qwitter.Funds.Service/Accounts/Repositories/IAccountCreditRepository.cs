
using Microsoft.EntityFrameworkCore;
using Qwitter.Funds.Service.Accounts.Models;

namespace Qwitter.Funds.Service.Accounts.Repositories;

public interface IAccountCreditRepository
{
    Task Insert(AccountCreditEntity accountCreditEntity);
    Task<AccountCreditEntity?> TryGetByExternalTransactionId(Guid externalTransactionId);
}

public class AccountCreditRepository : IAccountCreditRepository
{
    private readonly ServiceDbContext _dbContext;

    public AccountCreditRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(AccountCreditEntity accountCreditEntity)
    {
        await _dbContext.AccountCredits.AddAsync(accountCreditEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AccountCreditEntity?> TryGetByExternalTransactionId(Guid externalTransactionId)
    {
        return await _dbContext.AccountCredits
            .FirstOrDefaultAsync(x => x.ExternalTransactionId == externalTransactionId);
    }
}