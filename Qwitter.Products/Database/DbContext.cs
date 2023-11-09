using Microsoft.EntityFrameworkCore;

namespace Qwitter.Products.Database;

public class AppDbContext : DbContext
{
    // public DbSet<Post> Posts { get; set; }
    // public DbSet<Comment> Comments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
}
