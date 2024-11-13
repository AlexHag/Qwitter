using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Qwitter.Core.Application.Extensions;

namespace Qwitter.Core.Application.Authentication;

public static class AuthenticationServiceExtension
{
    public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var certificate = builder.GetJwtCertificate();

        if (certificate == null)
        {
            return builder;
        }

        var authConfigOptions = new AuthConfigOptions
        {
            Issuer = builder.Configuration["Issuer"]!,
            Audience = builder.Configuration["Audience"]!,
            Certificate = certificate
        };

        builder.Services.AddSingleton(authConfigOptions);

        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => 
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Issuer"]!,
                ValidAudience = builder.Configuration["Audience"]!,
                IssuerSigningKey = new X509SecurityKey(certificate),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        builder.Services.AddAuthorization(); 

        return builder;
    }

    private static X509Certificate2? GetJwtCertificate(this WebApplicationBuilder builder)
    {
        var clientCertificateConfiguration = builder.Configuration.GetSection("certificates")?.Get<Dictionary<string, string>>();

        if (clientCertificateConfiguration == null ||
            !clientCertificateConfiguration.TryGetValue("jwt_cert_path", out var jwtCertPath) ||
            !File.Exists(PathExtensions.AssemblyPath(jwtCertPath)))
        {
            return null;
        }

        if (clientCertificateConfiguration.TryGetValue("jwt_key_path", out var jwt_key_path) && File.Exists(PathExtensions.AssemblyPath(jwt_key_path)))
        {
            return X509Certificate2.CreateFromPemFile(PathExtensions.AssemblyPath(jwtCertPath), PathExtensions.AssemblyPath(jwt_key_path));
        }
        else
        {
            return new X509Certificate2(PathExtensions.AssemblyPath(jwtCertPath));
        };
    }
}