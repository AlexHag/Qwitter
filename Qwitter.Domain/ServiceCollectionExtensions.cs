using Microsoft.Extensions.DependencyInjection;
using Qwitter.Domain.Api;
using RestSharp;

namespace Qwitter.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserClient(this IServiceCollection services, string apiBaseAddress)
    {
        var restClient = new RestClient(apiBaseAddress);
        services.AddSingleton<IUserClient, UserClient>(p => new UserClient(restClient));
        return services;
    }

    public static IServiceCollection AddContentClient(this IServiceCollection services, string apiBaseAddress)
    {
        var restClient = new RestClient(apiBaseAddress);
        services.AddSingleton<IContentClient, ContentClient>(p => new ContentClient(restClient));
        return services;
    }
}