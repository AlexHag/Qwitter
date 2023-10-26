using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qwitter.Users.Services;

namespace Qwitter.Users;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddSingleton<AuthenticationService>(
            new AuthenticationService(
                X509Certificate2.CreateFromPemFile(configuration["Jwt:CertificatePath"]!),
                configuration["Jwt:Issuer"]!,
                configuration["Jwt:Audience"]!
            ));

        services.AddScoped<IUserService, UserService>();
        return services;
    }
}