using server.Models;
using Microsoft.EntityFrameworkCore;

namespace server.Context;

public class dataContext : DbContext
{
    private string connectionString = "Server=localhost, 1433;Database=hackday;User Id=SA;Password=hackday_password_xyz;TrustServerCertificate=true";

    public DbSet<User> Users { get; set; }
    public DbSet<Posts> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Username)
            .IsUnique(true);
        
        modelBuilder.Entity<Posts>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Comment>()
            .HasOne<Posts>()
            .WithMany()
            .HasForeignKey(p => p.RelatedPostId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}