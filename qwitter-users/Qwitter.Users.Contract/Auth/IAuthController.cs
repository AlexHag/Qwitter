using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Contract.Auth.Models;

namespace Qwitter.Users.Contract.Auth;

public interface IAuthController
{
    Task<ActionResult<AuthResponse>> Login(LoginRequest request);
    Task<ActionResult<AuthResponse>> Register(RegisterRequest request);
}
