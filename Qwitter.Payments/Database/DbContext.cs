using Microsoft.EntityFrameworkCore;
using Qwitter.Payment.Entities;

namespace Qwitter.Payment.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserWallet> UserWallets { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    // }
}
