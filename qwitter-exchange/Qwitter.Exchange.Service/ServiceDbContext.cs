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
        
        modelBuilder.Entity<FxRateEntity>()
            .Property(p => p.Rate)
            .HasPrecision(18, 18);
        
        // -------------------------------------------  

        modelBuilder.Entity<CurrencyAccountEntity>()
            .HasKey(p => p.CurrencyAccountId);
        
        modelBuilder.Entity<CurrencyAccountEntity>()
            .Property(p => p.Balance)
            .HasPrecision(18, 18);
        
        // -------------------------------------------  

        modelBuilder.Entity<FundExchangeEntity>()
            .HasKey(p => p.TransactionId);

        modelBuilder.Entity<FundExchangeEntity>()
            .Property(p => p.Rate)
            .HasPrecision(18, 18);
        
        modelBuilder.Entity<FundExchangeEntity>()
            .Property(p => p.SourceAmount)
            .HasPrecision(18, 18);
            
        modelBuilder.Entity<FundExchangeEntity>()
            .Property(p => p.DestinationAmount)
            .HasPrecision(18, 18);
    }
}