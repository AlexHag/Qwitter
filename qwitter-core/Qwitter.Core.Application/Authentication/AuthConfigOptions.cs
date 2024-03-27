using System.Security.Cryptography.X509Certificates;

namespace Qwitter.Core.Application.Authentication;

public class AuthConfigOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required X509Certificate2 Certificate { get; set; }
}