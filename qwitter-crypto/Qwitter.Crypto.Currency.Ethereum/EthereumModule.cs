
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Qwitter.Core.Application.Configuration;
using Qwitter.Crypto.Currency.Contract;
using Qwitter.Crypto.Currency.Contract.Wallets;

namespace Qwitter.Crypto.Currency.Ethereum;

public static class EthereumModule
{
    public static WebApplicationBuilder AddEthereumModule(this WebApplicationBuilder builder)
    {
        builder.AddConfiguration<AlchemyConfiguration>();

        builder.Services.AddHttpClient(Currencies.Ethereum, (services, client) =>
        {
            var configuration = services.GetRequiredService<AlchemyConfiguration>();
            client.BaseAddress = new Uri(configuration.BaseUrl);
        });

        builder.Services.AddKeyedScoped<ICryptoWalletService, EthereumWalletService>(Currencies.Ethereum);

        return builder;
    }
}