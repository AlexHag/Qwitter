using Qwitter.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Qwitter.Service.Database.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(string username, string password);
    Task<User?> LoginUser(string username, string password);
    Task<User?> GetUserById(Guid id);
}

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _dbContext;

    public UserRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateUser(string username, string password)
    {
        var salt = Helper.RandomString(16);
        var passwordHash = Helper.HashString(password + salt);

        var newUser = new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHash,
            Salt = salt,
            IsPremium = false
        };

        try 
        {
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return newUser;
        } catch (Exception e)
        {
            Console.WriteLine($"Error creating user: {e}");
            return null;
        }
    }

    public async Task<User?> LoginUser(string username, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Username == username);
        if (user is null) return null;

        var passwordHash = Helper.HashString(password + user.Salt);
        if (passwordHash == user.PasswordHash) return user;
        
        return null;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }
}