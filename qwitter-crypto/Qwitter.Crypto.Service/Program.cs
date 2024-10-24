using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application;
using Qwitter.Core.Application.Configuration;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Currency.Ethereum;
using Qwitter.Crypto.Service.Configuration;
using Qwitter.Crypto.Service.CryptoTransfer.Consumers;
using Qwitter.Crypto.Service.CryptoTransfer.Repositories;
using Qwitter.Crypto.Service.Wallet.Repositories;

namespace Qwitter.Crypto.Service;

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
        builder.AddConfiguration<CryptoConfig>();

        builder.Services.AddScoped<IMapper, Mapper>();
        builder.Services.AddScoped<IWalletRepository, WalletRepository>();
        builder.Services.AddScoped<ICryptoTransferRepository, CryptoTransferRepository>();
        builder.Services.AddScoped<ICryptoOutWalletRepository, CryptoOutWalletRepository>();

        builder.AddEthereumModule();

        builder.Services.AddDbContext<ServiceDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<ProcessCryptoTransferEventConsumer>(Name);
        builder.UseKafka();

        return builder;
    }

    public const string Name = "qwitter-crypto";
}