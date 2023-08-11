using Microsoft.EntityFrameworkCore;
using Qwitter.Models.Entities;

namespace Qwitter.Service.Database;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Username)
            .IsUnique(true);

        modelBuilder.Entity<Post>()
            .HasOne<User>()
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}