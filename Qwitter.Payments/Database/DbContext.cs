using Microsoft.EntityFrameworkCore;
using Qwitter.Payments.Entities;

namespace Qwitter.Payments.Database;

public class AppDbContext : DbContext
{
    public DbSet<UserWallet> UserWallets { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    // }
}
