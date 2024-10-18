using Microsoft.EntityFrameworkCore;
using Qwitter.Funds.Service.Accounts.Models;
using Qwitter.Funds.Service.Allocations.Models;

namespace Qwitter.Funds.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AllocationEntity> Allocations { get; set; }
    public DbSet<AccountCreditEntity> AccountCredits { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>()
            .HasKey(p => p.AccountId);
        
        modelBuilder.Entity<AccountEntity>()
            .Property(p => p.AvailableBalance)
            .HasPrecision(18, 18);

        modelBuilder.Entity<AccountEntity>()
            .Property(p => p.TotalBalance)
            .HasPrecision(18, 18);

        // ------------------------------------------------

        modelBuilder.Entity<AllocationEntity>()
            .HasKey(p => p.AllocationId);
        
        modelBuilder.Entity<AllocationEntity>()
            .HasIndex(p => p.TransactionId);
        
        modelBuilder.Entity<AllocationEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 18);
        
        modelBuilder.Entity<AllocationEntity>()
            .HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AllocationEntity>()
            .HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(p => p.DestinationAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        // ------------------------------------------------

        modelBuilder.Entity<AccountCreditEntity>()
            .HasKey(p => p.AccountCreditId);

        modelBuilder.Entity<AccountCreditEntity>()
            .HasIndex(p => p.ExternalTransactionId);

        modelBuilder.Entity<AccountCreditEntity>()
            .HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AccountCreditEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 18);
    }
}