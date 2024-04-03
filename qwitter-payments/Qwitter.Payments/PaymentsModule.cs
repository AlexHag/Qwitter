using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Kafka;
using Qwitter.Payments.Provider;
using Qwitter.Payments.Transactions.Consumers;
using Qwitter.Payments.Transactions.Repositories;
using Qwitter.Payments.Transactions.Services;
using Qwitter.Payments.Wallets.Repositories;
using Qwitter.Payments.Wallets.Services;

namespace Qwitter.Payments;

public static class PaymentsModule
{
    public static WebApplicationBuilder ConfigurePaymentsModule(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPaymentProviderService>(new PaymentProviderService(builder.Configuration["PaymentProviderUrl"]!));
        builder.Services.AddScoped<IWalletRepository, WalletRepository>();
        builder.Services.AddScoped<IWalletService, WalletService>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<TransactionCreatedConsumer>("payment-group");
        builder.RegisterConsumer<TransactionCompletedConsumer>("payment-group");
        
        builder.UseKafka();
        
        return builder;
    }
}