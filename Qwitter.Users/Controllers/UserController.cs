using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qwitter.Users.Database;
using Qwitter.Users.Models;
using Qwitter.Models.DTO;

namespace Qwitter.Users.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private static Random random = new Random();
    private readonly ILogger<UserController> _logger;
    private readonly AppDbContext _dbContext;

    public UserController(
        ILogger<UserController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UsernamePasswordDTO request)
    {
        var user = await _dbContext.Users.Where(p => p.Username == request.Username).FirstOrDefaultAsync();
        if (user is null)
        {
            return NotFound("Username not found");
        }
        
        var passwordHash = HashString(request.Password + user.Salt);
        if (passwordHash != user.PasswordHash)
        {
            BadRequest("Wrong password");
        }
        
        var userDTO = new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            IsPremium = user.IsPremium
        };
        return Ok(userDTO);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UsernamePasswordDTO request)
    {
        var existingUser = await _dbContext.Users.Where(p => p.Username == request.Username).FirstOrDefaultAsync();
        if (existingUser is not null)
        {
            return BadRequest("Username already exist");
        }

        var salt = RandomString(32);
        var passwordHash = HashString(request.Password + salt);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = passwordHash,
            Salt = salt,
            IsPremium = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        var userDTO = new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            IsPremium = user.IsPremium
        };
        return Ok(userDTO);
    }

    [HttpPatch]
    [Route("bio")]
    public async Task<IActionResult> UpdateBio(UpdateBioDTO request)
    {
        var user = await _dbContext.Users.FindAsync(request.UserId);
        if (user is null)
        {
            return NotFound("User not found");
        }
        user.Bio = request.Bio;
        user.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
        
        var userDTO = new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            IsPremium = user.IsPremium
        };
        return Ok(userDTO);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var userDTO = new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            Bio = user.Bio,
            IsPremium = user.IsPremium
        };
        return Ok(userDTO);
    }


    private string HashString(string input)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
