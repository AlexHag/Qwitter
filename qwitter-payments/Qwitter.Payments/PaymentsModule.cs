using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Configuration;
using Qwitter.Core.Application.Kafka;
using Qwitter.Payments.Provider;
using Qwitter.Payments.Provider.Configuration;
using Qwitter.Payments.Transactions.Configuration;
using Qwitter.Payments.Transactions.Consumers;
using Qwitter.Payments.Transactions.Repositories;
using Qwitter.Payments.Transactions.Services;
using Qwitter.Payments.User.Repositories;
using Qwitter.Payments.Wallets.Repositories;
using Qwitter.Payments.Wallets.Services;

namespace Qwitter.Payments;

public static class PaymentsModule
{
    public static WebApplicationBuilder ConfigurePaymentsModule(this WebApplicationBuilder builder)
    {
        builder.AddConfiguration<TransactionConfiguration>();
        builder.AddConfiguration<PaymentProviderCredentials>();
        builder.AddConfiguration<PaymentsConfiguration>();

        builder.Services.AddHttpClient<IPaymentProviderService, PaymentProviderService>((services, client) =>
        {
            var configuration = services.GetRequiredService<PaymentProviderCredentials>();
            client.BaseAddress = new Uri(configuration.BaseAddress);
        });
        
        builder.Services.AddScoped<IWalletRepository, WalletRepository>();
        builder.Services.AddScoped<IWalletService, WalletService>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<TransactionCreatedConsumer>("payment-group");
        builder.RegisterConsumer<TransactionCompletedConsumer>("payment-group");
        
        builder.UseKafka();
        
        return builder;
    }
}