using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Contract.Auth.Models;

namespace Qwitter.Users.Contract.Auth;

public interface IAuthController
{
    Task<AuthResponse> Login(LoginRequest request);
    Task<AuthResponse> Register(RegisterRequest request);
}
