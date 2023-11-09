using Microsoft.EntityFrameworkCore;
using Qwitter.Market.Entities;

namespace Qwitter.Market.Database;

public class AppDbContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<StockBuyOrder> StockBuyOrders { get; set; }
    public DbSet<StockSellOrder> StockSellOrders { get; set; }
    public DbSet<StockPosition> StockPositions { get; set; }
    public DbSet<StockBuyTransaction> StockBuyTransactions { get; set; }
    public DbSet<StockSellTransaction> StockSellTransaction { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    // }
}
