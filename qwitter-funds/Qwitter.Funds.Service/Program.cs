using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Repositories;
using Qwitter.Funds.Service.CurrencyExchange;
using Qwitter.Funds.Service.CurrencyExchange.Repositories;
using Qwitter.Funds.Service.ExchangeRate.Repositories;

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
        builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        builder.Services.AddScoped<ICurrencyExchangeRepository, CurrencyExchangeRepository>();
        builder.Services.AddScoped<ICurrencyAccountRepository, CurrencyAccountRepository>();
        builder.Services.AddScoped<IAccountCreditRepository, AccountCreditRepository>();

        builder.Services.AddScoped<ICurrencyExchangeActions, CurrencyExchangeActions>();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        return builder;
    }

    public const string Name = "qwitter-funds";
}