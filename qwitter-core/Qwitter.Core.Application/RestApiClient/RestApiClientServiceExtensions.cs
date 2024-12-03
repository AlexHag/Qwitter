using System.Net.Security;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qwitter.Core.Application.Extensions;

namespace Qwitter.Core.Application.RestApiClient;

public static class RestApiClientServiceExtensions
{
    public static WebApplicationBuilder AddRestApiClient<TController>(this WebApplicationBuilder builder)
    {
        var host = typeof(TController).GetCustomAttribute<ApiHostAttribute>();

        if (host == null)
        {
            throw new Exception($"{typeof(ApiHostAttribute).Name} is required for the controller interface {typeof(TController).Name}");
        }

        var serviceHosts = builder.Configuration.GetSection("service-hosts")?.Get<Dictionary<string, string>>();

        if (serviceHosts == null || !serviceHosts.TryGetValue(host.Port, out var hostPath))
        {
            throw new Exception($"Can not find service host for {host.Port}");
        }

        var httpClientHandler = new HttpClientHandler().ConfigureMtls(builder.Configuration);

        var httpClient = new HttpClient(httpClientHandler)
        {
            BaseAddress = new Uri(hostPath),
        };

        var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<TController>>();

        var client = new RestClientProxy<TController>().GetTransparentProxy(logger, httpClient);

        if (client is null)
        {
            throw new Exception($"Failed to create proxy for {typeof(TController).Name}");
        }

        builder.Services.AddSingleton(typeof(TController), client);

        return builder;
    }

    internal static HttpClientHandler ConfigureMtls(this HttpClientHandler httpClientHandler, IConfiguration configuration)
    {
        var certificate = configuration.GetClientMtlsCertificate();

        if (certificate == null)
        {
            return httpClientHandler;
        }

        httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
        httpClientHandler.SslProtocols = SslProtocols.Tls13;
        httpClientHandler.ServerCertificateCustomValidationCallback = ValidateServerCertificate;
        httpClientHandler.ClientCertificates.Add(certificate);

        return httpClientHandler;
    }

    private static X509Certificate2? GetClientMtlsCertificate(this IConfiguration configuration)
    {
        var clientCertificateConfiguration = configuration.GetSection("certificates")?.Get<Dictionary<string, string>>();

        if (clientCertificateConfiguration == null ||
            !clientCertificateConfiguration.TryGetValue("client_cert_path", out var clientCertPath) ||
            !File.Exists(PathExtensions.AssemblyPath(clientCertPath)))
        {
            return null;
        }

        if (clientCertificateConfiguration.TryGetValue("client_cert_password", out var clientCertPassword))
        {
            return new X509Certificate2(PathExtensions.AssemblyPath(clientCertPath), clientCertPassword);
        }
        else
        {
            return new X509Certificate2(PathExtensions.AssemblyPath(clientCertPath));
        }
    }

    private static bool ValidateServerCertificate(HttpRequestMessage request, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors errors)
    {
        // TODO: Implement server certificate validation
        return true;
    }
}