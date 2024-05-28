using Microsoft.EntityFrameworkCore;
using Qwitter.Crypto.Wallets.Models;
using Qwitter.Crypto.Wallets.Services;

namespace Qwitter.Crypto;

public class AppDbContext : DbContext
{
    public DbSet<WalletEntity> Wallets { get; set; }
    public DbSet<CryptoTransferEntity> CryptoTransfers { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WalletEntity>()
            .HasIndex(p => p.Address)
            .IsUnique()
            .HasDatabaseName("IX_Wallets_Address");
        
        modelBuilder.Entity<WalletEntity>()
            .Property(p => p.Balance)
            .HasPrecision(18, 6);
        
        modelBuilder.Entity<CryptoTransferEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 6);
        
        modelBuilder.Entity<CryptoTransferEntity>()
            .HasIndex(p => p.To)
            .HasDatabaseName("IX_CryptoTransfers_To");
    }
}
