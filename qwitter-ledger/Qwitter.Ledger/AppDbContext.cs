using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.BankAccount.Models;
using Qwitter.Ledger.Bank.Models;
using Qwitter.Ledger.ExchangeRates.Models;
using Qwitter.Ledger.Transactions.Models;
using Qwitter.Ledger.User.Models;
using Qwitter.Ledger.Invoices.Models;
using Qwitter.Ledger.Crypto.Models;
using Qwitter.Ledger.FundAllocations.Models;

namespace Qwitter.Ledger;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BankAccountEntity> BankAccounts { get; set; }
    public DbSet<BankInstitutionEntity> BankInstitutions { get; set; }
    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<InvoicePaymentEntity> InvoicePayments { get; set; }
    public DbSet<BankAccountCryptoWalletEntity> BankAccountCryptoWallets { get; set; }
    public DbSet<SystemBankAccountEntity> SystemBankAccounts { get; set; }
    public DbSet<FundAllocationEntity> FundAllocations { get; set; }
    public DbSet<BankAccountTransactionEntity> AccountTransactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(u => u.UserId);
        
        modelBuilder.Entity<BankAccountEntity>()
            .Property(p => p.Balance)
            .HasPrecision(18, 8);

        modelBuilder.Entity<ExchangeRateEntity>()
            .Property(p => p.Rate)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<FundAllocationEntity>()
            .Property(p => p.SourceAmount)
            .HasPrecision(18, 8);

        modelBuilder.Entity<FundAllocationEntity>()
            .Property(p => p.DestinationAmount)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<FundAllocationEntity>()
            .Property(p => p.ExchangeRate)
            .HasPrecision(18, 8);

        modelBuilder.Entity<FundAllocationEntity>()
            .Property(p => p.Fee)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<BankAccountTransactionEntity>()
            .Property(p => p.PreviousBalance)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<BankAccountTransactionEntity>()
            .Property(p => p.NewBalance)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<BankAccountTransactionEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 8);

        modelBuilder.Entity<InvoiceEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<InvoiceEntity>()
            .Property(p => p.AmountPayed)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<InvoicePaymentEntity>()
            .Property(p => p.Amount)
            .HasPrecision(18, 8);
        
        modelBuilder.Entity<BankAccountCryptoWalletEntity>()
            .HasIndex(p => p.WalletId)
            .HasDatabaseName("IX_BankAccountCryptoWallets_WalletId");
        
        modelBuilder.Entity<BankAccountCryptoWalletEntity>()
            .HasIndex(p => new { p.BankAccountId, p.Currency})
            .IsUnique()
            .HasDatabaseName("IX_BankAccountCryptoWallets_BankAccountId_Currency");
        
        modelBuilder.Entity<SystemBankAccountEntity>()
            .Property(p => p.Balance)
            .HasPrecision(18, 8);
    }
}
