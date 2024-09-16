using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"https://localhost:{host.Port}")
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

    public static WebApplicationBuilder AddRestApiClientV2<TController>(this WebApplicationBuilder builder)
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

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(hostPath)
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
}