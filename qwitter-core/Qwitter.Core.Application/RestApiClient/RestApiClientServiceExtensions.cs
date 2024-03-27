using Microsoft.AspNetCore.Builder;

namespace Qwitter.Core.Application.RestApiClient;

public static class RestApiClientServiceExtensions
{
    public static WebApplicationBuilder AddRestApiClient<TController>(this WebApplicationBuilder builder)
    {
        return builder;
    }
}