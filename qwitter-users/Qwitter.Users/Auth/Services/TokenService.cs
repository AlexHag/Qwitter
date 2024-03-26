using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Qwitter.Core.Application;
using Qwitter.Users.User.Models;

namespace Qwitter.Users.Auth.Services;

public class TokenService
{
    private AuthConfigOptions _auth;
    private readonly SigningCredentials _credentials;

    public TokenService(
        AuthConfigOptions auth)
    {
        _auth = auth;
        _credentials = new SigningCredentials(new X509SecurityKey(auth.Certificate), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateToken(UserEntity user)
    {
        var claims = new Claim[]
        {
            new("id", user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = _credentials,
            Issuer = _auth.Issuer,
            Audience = _auth.Audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
