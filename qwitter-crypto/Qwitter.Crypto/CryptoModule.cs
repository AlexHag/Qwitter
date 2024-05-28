using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Kafka;
using Qwitter.Crypto.Currency.Algorand.Wallets;
using Qwitter.Crypto.Currency.Contract;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Currency.Ethereum;
using Qwitter.Crypto.SystemLedger.Consumers;
using Qwitter.Crypto.Wallets.Repositories;
using Qwitter.Crypto.Wallets.Services;

namespace Qwitter.Crypto;

public static class CryptoModule
{
    public static WebApplicationBuilder ConfigureCryptoModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMapper, Mapper>();
        builder.Services.AddScoped<IWalletRepository, WalletRepository>();
        builder.Services.AddScoped<ICryptoTransferRepository, CryptoTransferRepository>();
        builder.Services.AddScoped<IWalletService, WalletService>();

        builder.AddEthereumModule();

        builder.Services.AddKeyedScoped<ICryptoWalletService, AlgorandWalletService>(Currencies.Algorand);

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<CryptoDepositConsumer>("payment-group");

        builder.UseKafka();
        
        return builder;
    }
}