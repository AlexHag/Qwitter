using Microsoft.EntityFrameworkCore;
using Qwitter.BankAccounts.Service.BankAccounts.Models;
using Qwitter.BankAccounts.Service.User.Models;

namespace Qwitter.BankAccounts.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BankAccountEntity> BankAccounts { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(p => p.UserId);

        // ----------------------------------------

        modelBuilder.Entity<BankAccountEntity>()
            .HasKey(p => p.BankAccountId);
        
        modelBuilder.Entity<BankAccountEntity>()
            .HasIndex(p => p.AccountNumber);
        
        modelBuilder.Entity<BankAccountEntity>()
            .HasIndex(p => p.UserId);

        modelBuilder.Entity<BankAccountEntity>()
            .Property(p => p.AvailableBalance)
            .HasPrecision(18, 18);
        
        modelBuilder.Entity<BankAccountEntity>()
            .Property(p => p.TotalBalance)
            .HasPrecision(18, 18);

        modelBuilder.Entity<BankAccountEntity>()
            .HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // ----------------------------------------
        
    }
}