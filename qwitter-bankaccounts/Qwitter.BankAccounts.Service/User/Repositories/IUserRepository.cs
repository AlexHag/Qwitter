
using Qwitter.BankAccounts.Service.User.Models;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.BankAccounts.Service.User.Repositories;

public interface IUserRepository
{
    Task Insert(UserEntity entity);
    Task Update(UserEntity entity);
    Task<UserEntity?> TryGetById(Guid userId);
    Task<UserEntity> GetById(Guid userId);
}

public class UserRepository : IUserRepository
{
    private readonly ServiceDbContext _dbContext;

    public UserRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(UserEntity entity)
    {
        await _dbContext.Users.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(UserEntity entity)
    {
        _dbContext.Users.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserEntity?> TryGetById(Guid userId)
        => await _dbContext.Users.FindAsync(userId);

    public async Task<UserEntity> GetById(Guid userId)
    {
        var entity = await _dbContext.Users.FindAsync(userId);

        if (entity == null)
        {
            throw new NotFoundApiException($"User {userId} not found");
        }

        return entity;
    }
}