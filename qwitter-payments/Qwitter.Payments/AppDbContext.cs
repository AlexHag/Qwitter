using Microsoft.EntityFrameworkCore;
using Qwitter.Payments.Transactions.Models;
using Qwitter.Payments.Wallets.Services;

namespace Qwitter.Payments;

public class AppDbContext : DbContext
{
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<WalletEntity> Wallets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }
}
