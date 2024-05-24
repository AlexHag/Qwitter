using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.Account.Models;
using Qwitter.Ledger.Bank.Models;
using Qwitter.Ledger.ExchangeRates.Models;
using Qwitter.Ledger.Transactions.Models;
using Qwitter.Ledger.User.Models;

namespace Qwitter.Ledger;

public class AppDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<BankInstitutionEntity> BankInstitutions { get; set; }
    public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(u => u.UserId);
    }
}
