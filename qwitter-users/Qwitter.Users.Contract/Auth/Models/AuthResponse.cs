using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.Contract.Auth.Models;

public class AuthResponse
{
    public required string Token { get; set; }
    public required UserProfile User { get; set; }
}