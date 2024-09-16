
using Qwitter.Funds.Service.Accounts.Models;

namespace Qwitter.Funds.Service.Accounts.Repositories;

public interface IAccountCreditRepository
{
    Task Insert(AccountCreditEntity accountCreditEntity);
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
}