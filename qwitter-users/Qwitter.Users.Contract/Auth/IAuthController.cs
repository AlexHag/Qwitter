using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Users.Contract.Auth.Models;

namespace Qwitter.Users.Contract.Auth;

[ApiHost(Host.Port, "auth")]
public interface IAuthController
{
    [HttpPost("login")]
    Task<AuthResponse> Login(LoginRequest request);
    [HttpPost("register")]
    Task<AuthResponse> Register(RegisterRequest request);
}
