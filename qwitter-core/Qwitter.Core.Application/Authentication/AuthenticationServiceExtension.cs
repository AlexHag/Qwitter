using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Qwitter.Core.Application.Authentication;

public static class AuthenticationServiceExtension
{
    public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var certificate = new X509Certificate2(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "cert.pfx"));

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
}