using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Posts.Models;

namespace Qwitter.Content;

public class AppDbContext : DbContext
{
    public DbSet<PostEntity> Posts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PostEntity>()
            .HasIndex(p => p.UserId);
    }
}
