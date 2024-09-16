using Microsoft.AspNetCore.Mvc;
using Qwitter.User.Contract.Auth;
using Qwitter.User.Contract.Auth.Models;

namespace Qwitter.Users.Api.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public Task<AuthResponse> Login(LoginRequest reques)
    {
        return _authService.Login(reques);
    }

    [HttpPost("register")]
    public Task<AuthResponse> Register(RegisterRequest request)
    {
        return _authService.Register(request);
    }
}