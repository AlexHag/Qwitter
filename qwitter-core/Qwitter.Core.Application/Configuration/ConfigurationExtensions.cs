using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Qwitter.Core.Application.Configuration;

public static class ConfigurationExtensions
{
    public static WebApplicationBuilder AddConfiguration<TConfiguration>(this WebApplicationBuilder builder)
        where TConfiguration : class, new()
    {
        var attribute = typeof(TConfiguration).GetCustomAttribute<ConfigurationAttribute>();

        if (attribute is null)
        {
            throw new Exception($"Configuration attribute not found on type {typeof(TConfiguration).Name}");
        }

        var instance = Activator.CreateInstance<TConfiguration>();

        if (instance is null)
        {
            throw new Exception($"Failed to create instance of {typeof(TConfiguration).Name}");
        }

        builder.Configuration.GetSection(attribute.Path).Bind(instance);
        builder.Services.AddSingleton(instance);

        return builder;
    }
}
