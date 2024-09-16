using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.User.Contract.Auth.Models;

namespace Qwitter.User.Contract.Auth;

[ApiHost(Host.Name, "auth")]
public interface IAuthService
{
    [HttpPost("login")]
    Task<AuthResponse> Login(LoginRequest request);

    [HttpPost("register")]
    Task<AuthResponse> Register(RegisterRequest request);
}
