using Microsoft.EntityFrameworkCore;
using Qwitter.User.Service.User.Models;

namespace Qwitter.User.Service;

public interface IUserRepository
{
    Task Insert(UserEntity user);
    Task<UserEntity?> GetById(Guid userId);
    Task<UserEntity?> GetByEmail(string email);
}

public class UserRepository : IUserRepository
{
    private readonly ServiceDbContext _context;

    public UserRepository(ServiceDbContext context)
    {
        _context = context;
    }

    public async Task Insert(UserEntity user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> GetById(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}