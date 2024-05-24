using Microsoft.EntityFrameworkCore;
using Qwitter.Payments.Transactions.Models;
using Qwitter.Payments.User.Models;
using Qwitter.Payments.Wallets.Services;

namespace Qwitter.Payments;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<WalletEntity> Wallets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(u => u.UserId);
    }
}
