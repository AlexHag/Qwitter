using Qwitter.Content.Users.Models;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Content.Users.Repositories;

public interface IUserRepository
{
    Task<UserEntity> InsertUser(UserInsertModel model);
    Task<UserEntity> UpdateUser(UserUpdateModel model);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserEntity> InsertUser(UserInsertModel model)
    {
        var user = new UserEntity
        {
            UserId = model.UserId,
            Username = model.Username,
            HasPremium = false,
            CreatedAt = DateTime.UtcNow,
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<UserEntity> UpdateUser(UserUpdateModel user)
    {
        var entity = await _dbContext.Users.FindAsync(user.UserId)
            ?? throw new NotFoundApiException("User not found");
        
        if (user.Username != null)
            entity.Username = user.Username;
        
        if (user.HasPremium != null)
            entity.HasPremium = user.HasPremium.Value;
        
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return entity;
    }
}