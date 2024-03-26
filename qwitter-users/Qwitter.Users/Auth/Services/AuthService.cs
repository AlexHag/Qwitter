using Qwitter.Users.Contract.Auth.Models;
using Qwitter.Users.User.Models;

namespace Qwitter.Users.Auth.Services;

public static class UserPasswordHasing
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

    public static bool VerifyPassword(this UserEntity user, string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, user.Salt);
        return BCrypt.Net.BCrypt.Verify(user.Password, hashedPassword);
    }
}