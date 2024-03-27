using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Qwitter.Core.Application.Authentication;

public interface ITokenService
{
    public string GenerateToken(Guid userId, params Claim[] additionalClaims);
}

public class TokenService : ITokenService
{
    private AuthConfigOptions _auth;
    private readonly SigningCredentials _credentials;

    public TokenService(
        AuthConfigOptions auth)
    {
        _auth = auth;
        _credentials = new SigningCredentials(new X509SecurityKey(auth.Certificate), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateToken(Guid userId, params Claim[] additionalClaims)
    {
        var claims = new List<Claim>
        {
            new("id", userId.ToString())
        };

        claims.AddRange(additionalClaims);

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