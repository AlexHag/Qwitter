using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.BankAccount.Models;
using Qwitter.Ledger.Bank.Models;
using Qwitter.Ledger.ExchangeRates.Models;
using Qwitter.Ledger.Transactions.Models;
using Qwitter.Ledger.User.Models;
using Qwitter.Ledger.Invoices.Models;

namespace Qwitter.Ledger;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BankAccountEntity> BankAccounts { get; set; }
    public DbSet<BankInstitutionEntity> BankInstitutions { get; set; }
    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<InvoiceEntity> Invoices { get; set; }
    public DbSet<InvoicePaymentEntity> InvoicePayments { get; set; }

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

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.PreviousBalance)
            .HasPrecision(18, 8);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.NewBalance)
            .HasPrecision(18, 8);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.SourceAmount)
            .HasPrecision(18, 8);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.DestinationAmount)
            .HasPrecision(18, 8);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.ExchangeRate)
            .HasPrecision(18, 8);

        modelBuilder.Entity<TransactionEntity>()
            .Property(p => p.Fee)
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
    }
}
