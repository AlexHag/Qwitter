using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Contract.Auth;
using Qwitter.Users.Contract.Auth.Models;
using Qwitter.Users.Repositories.User;
using Qwitter.Users.Auth.Services;
using Qwitter.Core.Application.Kafka;
using MapsterMapper;
using Qwitter.Users.Contract.User.Events;
using Qwitter.Core.Application.Authentication;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Users.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IEventProducer _eventProducer;
    private readonly IMapper _mapper;

    public AuthController(
        IUserRepository userRepository,
        ITokenService tokenService,
        IEventProducer eventProducer,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _eventProducer = eventProducer;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _userRepository.GetUserByUsernameOrEmail(request.UsernameOrEmail);

        if (user is null)
        {
            throw new BadRequestApiException("User not found");
        }

        if (!AuthExtensions.VerifyPassword(request.Password, user.Password))
        {
            throw new BadRequestApiException("Invalid password");
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
        if (await _userRepository.GetUserByEmail(request.Email) is not null)
        {
            throw new BadRequestApiException("Email already exists");
        }

        if (await _userRepository.GetUserByEmail(request.Email) is not null)
        {
            throw new BadRequestApiException("Email already exists");
        }

        var user = await _userRepository.InsertUser(request.HashPassword());

        await _eventProducer.Produce(_mapper.Map<UserCreatedEvent>(user));

        var token = _tokenService.GenerateToken(user.UserId);

        return new AuthResponse
        {
            Token = token
        };
    }
}