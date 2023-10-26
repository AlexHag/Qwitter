using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Qwitter.Data;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Could not get connectionstring bruh");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)); //, db => db.MigrationsAssembly(typeof(MySqlDbContextFactory).Assembly.GetName().Name)));
        
        return services;
    }
}