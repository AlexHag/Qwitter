using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.User.Models;

namespace Qwitter.Users.Repositories.User;

public interface IUserRepository
{
    Task<UserEntity?> GetUserById(Guid userId);
    Task<UserEntity?> GetUserByUsername(string username);
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity?> GetUserByUsernameOrEmail(string usernameOrEmail);
    Task<UserEntity> InsertUser(UserInsertModel user);
    Task<UserEntity> UpdateUser(UserUpdateModel user);
    Task<UserEntity> DeleteUser(Guid userId);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserEntity> InsertUser(UserInsertModel userInsert)
    {
        var userEntity = new UserEntity
        {
            UserId = Guid.NewGuid(),
            Email = userInsert.Email,
            Username = userInsert.Username,
            Password = userInsert.Password,
            CreatedAt = DateTime.Now,
            UserState = UserState.Created
        };

        var user = await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();

        return user.Entity;
    }

    public async Task<UserEntity> DeleteUser(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId) ?? throw new Exception("User not found");
        user.UserState = UserState.Deleted;
        user.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity?> GetUserById(Guid userId)
    {
        return await _dbContext.Users.FindAsync(userId);
    }

    public async Task<UserEntity?> GetUserByUsername(string username)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<UserEntity> UpdateUser(UserUpdateModel user)
    {
        var entity = await _dbContext.Users.FindAsync(user.UserId) ?? throw new NotFoundApiException("User not found");
        
        if (user.Email != null)
            entity.Email = user.Email;

        if (user.Username != null)
            entity.Username = user.Username;
        
        if (user.Password != null)
            entity.Password = user.Password;
        
        if (user.HasPremium != null)
            entity.HasPremium = user.HasPremium.Value;
        
        if (user.FollowerCount != null)
            entity.FollowerCount = user.FollowerCount.Value;
        
        if (user.FollowingCount != null)
            entity.FollowingCount = user.FollowingCount.Value;
        
        if (user.UserState != null)
            entity.UserState = user.UserState.Value;
        
        entity.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<UserEntity?> GetUserByUsernameOrEmail(string usernameOrEmail)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
    }
}