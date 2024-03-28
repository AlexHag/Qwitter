using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Qwitter.Core.Application.RestApiClient;

public static class RestApiClientServiceExtensions
{
    public static WebApplicationBuilder AddRestApiClient<TController>(this WebApplicationBuilder builder)
    {
        var client = new RestClientProxy<TController>().GetTransparentProxy();

        if (client is null)
        {
            throw new Exception($"Failed to create proxy for {typeof(TController).Name}");
        }

        builder.Services.AddSingleton(typeof(TController), client);

        return builder;
    }
}