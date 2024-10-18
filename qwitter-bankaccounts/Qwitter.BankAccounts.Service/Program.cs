using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.BankAccounts.Service.BankAccounts.Repositorie;
using Qwitter.BankAccounts.Service.BankAccounts.Services;
using Qwitter.BankAccounts.Service.User.Consumers;
using Qwitter.BankAccounts.Service.User.Repositories;
using Qwitter.Core.Application;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Accounts;

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
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

        builder.Services.AddScoped<IAccountNumberGenerator, AccountNumberGenerator>();

        builder.AddRestApiClientV2<IAccountService>();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<UserCreatedConsumer>(Name);
        builder.RegisterConsumer<UserVerifiedConsumer>(Name);
        builder.UseKafka();

        return builder;
    }

    public const string Name = "qwitter-bankaccounts";
}