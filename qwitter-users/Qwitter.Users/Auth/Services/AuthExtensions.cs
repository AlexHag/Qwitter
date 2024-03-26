using System.Security.Claims;
using Qwitter.Users.Contract.Auth.Models;
using Qwitter.Users.User.Models;

namespace Qwitter.Users.Auth.Services;

public static class AuthExtensions
{
    public static UserInsertModel HashPassword(this RegisterRequest request)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

        return new UserInsertModel
        {
            Email = request.Email,
            Username = request.Username,
            Password = hashedPassword,
            Salt = salt
        };
    }

    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("id"));
    }
}