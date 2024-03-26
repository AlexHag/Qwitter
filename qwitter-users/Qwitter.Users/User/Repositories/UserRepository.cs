using Microsoft.EntityFrameworkCore;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.User.Models;

namespace Qwitter.Users.Repositories.User;

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
            Salt = userInsert.Salt,
            CreatedAt = DateTime.Now,
            UserState = UserState.Created
        };

        var user = await _dbContext.Users.AddAsync(userEntity);
        await _dbContext.SaveChangesAsync();

        return user.Entity;
    }

    public async Task<UserEntity> DeleteUser(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

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

    public Task<UserEntity> UpdateUser(UserEntity user)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEntity?> GetUserByUsernameOrEmail(string usernameOrEmail)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
    }
}