using System.Reflection;
using Microsoft.AspNetCore.Builder;
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

        var client = new RestClientProxy<TController>(logger, httpClient).GetTransparentProxy();

        if (client is null)
        {
            throw new Exception($"Failed to create proxy for {typeof(TController).Name}");
        }

        builder.Services.AddSingleton(typeof(TController), client);

        return builder;
    }
}