using Microsoft.EntityFrameworkCore;
using Qwitter.Crypto.Service.CryptoTransfer.Models;
using Qwitter.Crypto.Service.Wallet.Models;

namespace Qwitter.Crypto.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<WalletEntity> Wallets { get; set; }
    public DbSet<CryptoTransferEntity> CryptoTransfers { get; set; }
    public DbSet<CryptoOutWalletEntity> CryptoOutWallets { get; set; }
 
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
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

        // ------------------------------------------

        modelBuilder.Entity<CryptoTransferEntity>()
            .HasKey(p => p.TransactionId);
        
        modelBuilder.Entity<CryptoTransferEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 6);
        
        modelBuilder.Entity<CryptoTransferEntity>()
            .HasIndex(p => p.DestinationAddress)
            .HasDatabaseName("IX_CryptoTransfers_To");

        modelBuilder.Entity<CryptoTransferEntity>()
            .Property(p => p.Fee)
            .HasPrecision(18, 6);

        // ------------------------------------------

        modelBuilder.Entity<CryptoOutWalletEntity>()
            .HasOne<WalletEntity>()
            .WithMany()
            .HasForeignKey(p => p.WalletId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}