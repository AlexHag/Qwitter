using Microsoft.EntityFrameworkCore;
using Qwitter.Payments.User.Models;

namespace Qwitter.Payments.User.Repositories;

public interface IUserRepository
{
    Task Insert(UserEntity userEntity);
    Task<UserEntity?> GetById(Guid userId);
    Task Update(UserEntity user);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<UserEntity?> GetById(Guid userId)
    {
        return _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task Insert(UserEntity userEntity)
    {
        await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(UserEntity user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}