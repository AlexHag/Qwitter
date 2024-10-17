using Microsoft.EntityFrameworkCore;
using Qwitter.Exchange.Service.CurrencyAccounts.Models;
using Qwitter.Exchange.Service.FundExchange.Models;
using Qwitter.Exchange.Service.Rate.Models;

namespace Qwitter.Exchange.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<FxRateEntity> FxRates { get; set; }
    public DbSet<CurrencyAccountEntity> CurrencyAccounts { get; set; }
    public DbSet<FundExchangeEntity> FundExchanges { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FxRateEntity>()
            .HasKey(p => p.FxRateId);

        modelBuilder.Entity<CurrencyAccountEntity>()
            .HasKey(p => p.CurrencyAccountId);
        
        modelBuilder.Entity<FundExchangeEntity>()
            .HasKey(p => p.TransactionId);
    }
}