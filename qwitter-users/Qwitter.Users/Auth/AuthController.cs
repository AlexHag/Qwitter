using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Contract.Auth;
using Qwitter.Users.Contract.Auth.Models;
using Qwitter.Users.Repositories.User;
using Qwitter.Users.Auth.Services;

namespace Qwitter.Users.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthController(
        IUserRepository userRepository,
        TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _userRepository.GetUserByUsernameOrEmail(request.UsernameOrEmail);

        if (user is null)
        {
            return BadRequest("User not found");
        }

        if (!user.VerifyPassword(request.Password))
        {
            return BadRequest("Invalid password");
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await _userRepository.GetUserByEmail(request.Email) is not null)
        {
            return BadRequest("Email already exists");
        }

        if (await _userRepository.GetUserByEmail(request.Email) is not null)
        {
            return BadRequest("Username already exists");
        }
        
        var user = await _userRepository.InsertUser(request.HashPassword());

        var token = _tokenService.GenerateToken(user);

        return new AuthResponse
        {
            Token = token
        };
    }
}