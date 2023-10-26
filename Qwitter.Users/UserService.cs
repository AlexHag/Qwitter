using Microsoft.EntityFrameworkCore;
using Qwitter.Users.Exceptions;
using Qwitter.Data;
using Qwitter.Data.Entities;
using Qwitter.Users.Requests;
using Qwitter.Users.Responses;
using Qwitter.Users.Services;

namespace Qwitter.Users;

public interface IUserService
{
    Task<string> Login(UsernamePasswordRequest request);
    Task<string> Register(UsernamePasswordRequest request);
    Task<UserDTO> GetUserById(Guid Id);
    Task MakeUserPremium(Guid userId);
}


public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly AuthenticationService _authService;

    public UserService(
        AppDbContext dbContext,
        AuthenticationService authService)
    {
        _dbContext = dbContext;
        _authService = authService;
    }

    public async Task<string> Login(UsernamePasswordRequest request)
    {
        var user = await _dbContext.Users.Where(p => p.Username == request.Username).FirstOrDefaultAsync();
        if (user is null)
            throw new LoginFailedException("Invalid username");

        var passwordHash = _authService.HashString(request.Password + user.Salt);
        if (passwordHash != user.PasswordHash)
            throw new LoginFailedException("Wrong password");

        var jwt = _authService.CreateJwt(user.Id);
        return jwt;
    }

    public async Task<string> Register(UsernamePasswordRequest request)
    {
        var existingUser = await _dbContext.Users.Where(p => p.Username == request.Username).FirstOrDefaultAsync();
        if (existingUser is not null)
            throw new UsernameAlreadyExistException();
        
        var salt = _authService.RandomString(32);
        var passwordHash = _authService.HashString(request.Password + salt);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = passwordHash,
            Salt = salt,
            IsPremium = false
        };

        await _dbContext.Users.AddAsync(user);

        var jwt = _authService.CreateJwt(user.Id);
        await _dbContext.SaveChangesAsync();
        return jwt;
    }

    public async Task<UserDTO> GetUserById(Guid Id)
    {
        var user = await _dbContext.Users.FindAsync(Id);
        if (user is null)
            throw new NotFoundException("User not found");

        return new UserDTO
        {
            Username = user.Username,
            IsPremium = user.IsPremium
        };
    }

    public async Task MakeUserPremium(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user is null)
            throw new NotFoundException("User not found");
        user.IsPremium = true;
        await _dbContext.SaveChangesAsync();
    }
}
