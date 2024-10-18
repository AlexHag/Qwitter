using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.User.Contract.Auth;
using Qwitter.User.Contract.Auth.Models;
using Qwitter.User.Contract.Events;
using Qwitter.User.Contract.User.Models;
using Qwitter.User.Service.User.Models;

namespace Qwitter.User.Service.Auth;

[ApiController]
[Route("auth")]
public class AuthService : ControllerBase, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IEventProducer _eventProducer;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IEventProducer eventProducer)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _eventProducer = eventProducer;
    }

    [HttpPost("login")]
    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _userRepository.TryGetByEmail(request.Email);

        if (user == null)
        {
            throw new UnauthorizedApiException("Invalid email or password");
        }

        var success = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

        if (!success)
        {
            // TODO: Record invalid login attempt
            throw new UnauthorizedApiException("Invalid email or password");
        }

        var token = _tokenService.GenerateToken(user.UserId);

        return new AuthResponse
        {
            Token = token
        };
    }

    [HttpPost("register")]
    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        // TODO: Validate password policy

        var existingUser = await _userRepository.TryGetByEmail(request.Email);

        if (existingUser != null)
        {
            throw new ConflictApiException("Email already exists");
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        var user = new UserEntity
        {
            UserId = Guid.NewGuid(),
            Email = request.Email,
            Password = passwordHash,
            UserState = UserState.Unverified,
            CreatedAt = DateTime.UtcNow,
        };

        await _userRepository.Insert(user);

        await _eventProducer.Produce(new UserCreatedEvent
        {
            UserId = user.UserId,
            Email = user.Email
        });

        var token = _tokenService.GenerateToken(user.UserId);

        return new AuthResponse
        {
            Token = token
        };
    }
}