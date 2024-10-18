using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application;

namespace Qwitter.BankAccounts.Service;

public static class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureServices()
            .Build()
            .ConfigureApp()
            .Run();

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMapper, Mapper>();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        return builder;
    }

    public const string Name = "qwitter-funds";
}