using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Qwitter.Web.Services;

public class AuthenticationService
{
    private static Random random = new Random();
    private readonly X509Certificate2 _certificate;
    private readonly SigningCredentials _credentials;
    private readonly string _issuer;
    private readonly string _audience;

    public AuthenticationService(X509Certificate2 certificate, string issuer, string audience)
    {
        _certificate = certificate;
        _credentials = new SigningCredentials(new X509SecurityKey(_certificate), SecurityAlgorithms.RsaSha256);
        _issuer = issuer;
        _audience = audience;
    }

    public string CreateJwt(Guid id)
    {
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim(ClaimTypes.Email, "johndoe@example.com"),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Id", id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = _credentials,
            Issuer = _issuer,
            Audience = _audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string HashString(string input)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(input));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    public string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static Guid GetUserIdFromContext(HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        IEnumerable<Claim> claims = identity!.Claims; 
        var claimId = identity.FindFirst("Id")?.Value;
        return Guid.Parse(claimId!);
    }
}