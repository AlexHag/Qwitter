using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application;
using Qwitter.Core.Application.Kafka;
using Qwitter.Funds.Service.Accounts.Repositories;
using Qwitter.Funds.Service.Allocations.Repositories;
using Qwitter.Funds.Service.Clients.Repositories;
using Qwitter.Funds.Service.Transactions.Handler;
using Qwitter.Funds.Service.Transactions.Repositories;

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
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<ITransactionHandler, TransactionHandler>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));
        
        builder.UseKafka();

        return builder;
    }

    public const string Name = "qwitter-funds";
}