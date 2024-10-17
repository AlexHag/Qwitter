using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Exchange.Service.CurrencyAccounts.Repositories;
using Qwitter.Exchange.Service.FundExchange.Repositories;
using Qwitter.Exchange.Service.Rate.Repositories;
using Qwitter.Funds.Contract.Allocations;

namespace Qwitter.Exchange.Service;

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

        builder.Services.AddScoped<IFxRateRepository, FxRateRepository>();
        builder.Services.AddScoped<ICurrencyAccountRepository, CurrencyAccountRepository>();
        builder.Services.AddScoped<IFundExchangeRepository, FundExchangeRepository>();

        builder.AddRestApiClientV2<IAllocationService>();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        return builder;
    }

    public const string Name = "qwitter-exchange";
}