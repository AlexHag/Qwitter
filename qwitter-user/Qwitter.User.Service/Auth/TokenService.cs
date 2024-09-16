
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Qwitter.Core.Application.Authentication;

public interface ITokenService
{
    public string GenerateToken(Guid userId, params Claim[] additionalClaims);
}

public class TokenService : ITokenService
{
    private readonly AuthConfigOptions _options;

    public TokenService(AuthConfigOptions options)
    {
        _options = options;
    }

    public string GenerateToken(Guid userId, params Claim[] additionalClaims)
    {
        var key = new X509SecurityKey(_options.Certificate);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var claims = new List<Claim>
        {
            new ("id", userId.ToString())
        };

        claims.AddRange(additionalClaims);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _options.Issuer,
            Audience = _options.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}