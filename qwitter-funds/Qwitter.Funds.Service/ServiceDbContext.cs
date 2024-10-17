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

        modelBuilder.Entity<AllocationEntity>()
            .HasKey(p => p.AllocationId);
        
        modelBuilder.Entity<AllocationEntity>()
            .HasIndex(p => p.TransactionId);

        modelBuilder.Entity<AccountCreditEntity>()
            .HasKey(p => p.AccountCreditId);
    }
}