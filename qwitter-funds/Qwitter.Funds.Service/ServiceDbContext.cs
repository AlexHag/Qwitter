using Microsoft.EntityFrameworkCore;
using Qwitter.Funds.Service.Accounts.Models;
using Qwitter.Funds.Service.Allocations.Models;
using Qwitter.Funds.Service.Clients.Models;

namespace Qwitter.Funds.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<AccountEntity> Accounts { get; set; }
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

        // ------------------------------------------------

        modelBuilder.Entity<AllocationEntity>()
            .HasKey(p => p.AllocationId);

        modelBuilder.Entity<AllocationEntity>()
            .HasIndex(p => p.ExternalSourceTransactionId);

        modelBuilder.Entity<AllocationEntity>()
            .Property(p => p.Amount)
            .HasPrecision(36, 18);
        
        modelBuilder.Entity<AllocationEntity>()
            .HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(p => p.SourceAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AllocationEntity>()
            .HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(p => p.DestinationAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        // ------------------------------------------------

        modelBuilder.Entity<ClientEntity>()
            .HasKey(p => p.ClientId);
    }
}