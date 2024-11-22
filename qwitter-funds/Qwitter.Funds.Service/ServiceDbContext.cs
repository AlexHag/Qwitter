using Microsoft.EntityFrameworkCore;
using Qwitter.Funds.Service.Accounts.Models;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.Clients.Models;
using Qwitter.Funds.Service.Transactions.Models;

namespace Qwitter.Funds.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<AllocationEntity> Allocations { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>()
            .HasKey(p => p.AccountId);
        
        modelBuilder.Entity<AccountEntity>()
            .Property(p => p.Balance)
            .HasPrecision(36, 18);
        
        modelBuilder.Entity<AccountEntity>()
            .HasIndex(p => p.ExternalAccountId);

        // ------------------------------------------------

        modelBuilder.Entity<AllocationEntity>()
            .HasKey(p => p.AllocationId);

        modelBuilder.Entity<AllocationEntity>()
            .Property(p => p.Amount)
            .HasPrecision(36, 18);
        
        modelBuilder.Entity<AllocationEntity>()
            .HasIndex(p => p.ExternalSourceTransactionId);

        // ------------------------------------------------

        modelBuilder.Entity<ClientEntity>()
            .HasKey(p => p.ClientId);
        
        modelBuilder.Entity<ClientEntity>()
            .HasIndex(p => p.ClientCertificateThumbprint);

        // ------------------------------------------------

        modelBuilder.Entity<TransactionEntity>()
            .HasKey(p => p.TransactionId);
        
        modelBuilder.Entity<TransactionEntity>()
            .HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TransactionEntity>()
            .HasOne<AllocationEntity>()
            .WithMany()
            .HasForeignKey(p => p.AllocationId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<TransactionEntity>()
            .HasIndex(p => p.AllocationId);
        
        modelBuilder.Entity<TransactionEntity>()
            .HasIndex(p => p.AccountId);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.Amount)
            .HasPrecision(36, 18);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.PreviousBalance)
            .HasPrecision(36, 18);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.NewBalance)
            .HasPrecision(36, 18);

        // ------------------------------------------------
    }
}