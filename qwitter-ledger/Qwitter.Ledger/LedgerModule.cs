using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Configuration;
using Qwitter.Core.Application.Kafka;
using Qwitter.Ledger.BankAccount.Configuration;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.BankAccount.Services;
using Qwitter.Ledger.Bank.Repositories;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.Transactions.Repositories;
using Qwitter.Ledger.Transactions.Services;
using Qwitter.Ledger.User.Repositories;
using Qwitter.Ledger.User.Consumers;
using MapsterMapper;
using Qwitter.Ledger.Invoices.Repositories;
using Qwitter.Ledger.Invoices.Services;
using Qwitter.Ledger.Invoices.Consumers;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Crypto.Contract.Wallets;
using Qwitter.Ledger.Crypto.Repositories;
using Qwitter.Ledger.Crypto.Services;
using Qwitter.Ledger.Crypto.Consumers;
using Qwitter.Ledger.FundAllocations.Repositories;

namespace Qwitter.Ledger;

public static class LedgerModule
{
    public static WebApplicationBuilder ConfigureLedgerModule(this WebApplicationBuilder builder)
    {
        builder.AddConfiguration<BankConfiguration>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.Services.AddScoped<IMapper, Mapper>();
        builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
        builder.Services.AddScoped<IBankAccountService, BankAccountService>();
        builder.Services.AddScoped<IBankInstitutionRepository, BankInstitutionRepository>();
        builder.Services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        builder.Services.AddScoped<IInvoicePaymentRepository, InvoicePaymentRepository>();
        builder.Services.AddScoped<IInvoiceService, InvoiceService>();
        builder.Services.AddScoped<IBankAccountCryptoWalletRepository, BankAccountCryptoWalletRepository>();
        builder.Services.AddScoped<ICryptoService, CryptoService>();

        builder.Services.AddScoped<IBankAccountTransactionRepository, BankAccountTransactionRepository>();

        builder.Services.AddScoped<ISystemBankAccountRepository, SystemBankAccountRepository>();
        builder.Services.AddScoped<IAllocationCurrencyExchangeService, AllocationCurrencyExchangeService>();
        builder.Services.AddScoped<ISystemTransactionService, SystemTransactionService>();
        builder.Services.AddScoped<IFundAllocationRepository, FundAllocationRepository>();

        // builder.RegisterConsumer<TransactionCompletedConsumer>("ledger-group");
        builder.RegisterConsumer<CryptoDepositEventConsumer>(App.Name);
        builder.RegisterConsumer<UserCreatedConsumer>(App.Name);
        builder.RegisterConsumer<UserStateChangedConsumer>(App.Name);
        builder.RegisterConsumer<InvoiceOverpayedConsumer>(App.Name);

        builder.AddRestApiClient<IWalletController>();

        builder.UseKafka();
        
        return builder;
    }
}