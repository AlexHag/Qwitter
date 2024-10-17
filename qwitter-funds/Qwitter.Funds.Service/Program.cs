using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Repositories;

namespace Qwitter.Funds.Service;

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

        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IAllocationRepository, AllocationRepository>();
        builder.Services.AddScoped<IAccountCreditRepository, AccountCreditRepository>();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        return builder;
    }

    public const string Name = "qwitter-funds";
}