using Microsoft.EntityFrameworkCore;
using Qwitter.Funds.Service.Accounts.Models;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.CurrencyExchange.Models;
using Qwitter.Funds.Service.ExchangeRate.Models;

namespace Qwitter.Funds.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AllocationEntity> Allocations { get; set; }
    public DbSet<AccountCreditEntity> AccountCredits { get; set; }

    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
    public DbSet<CurrencyExchangeEntity> CurrencyExchanges { get; set; }
    public DbSet<CurrencyAccountEntity> CurrencyAccounts { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllocationEntity>()
            .HasKey(p => p.AllocationId);

        modelBuilder.Entity<AccountEntity>()
            .HasKey(p => p.AccountId);

        modelBuilder.Entity<AccountCreditEntity>()
            .HasKey(p => p.AccountCreditId);

        modelBuilder.Entity<ExchangeRateEntity>()
            .HasKey(p => p.ExchangeRateId);

        modelBuilder.Entity<CurrencyExchangeEntity>()
            .HasKey(p => p.CurrencyExchangeId);
        
        modelBuilder.Entity<CurrencyAccountEntity>()
            .HasKey(p => p.Currency);
    }
}