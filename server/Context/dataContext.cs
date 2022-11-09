using server.Models;
using Microsoft.EntityFrameworkCore;

namespace server.Context;

public class dataContext : DbContext
{
    private string connectionString = "Server=localhost, 1433;Database=hackday;User Id=SA;Password=hackday_password_xyz;TrustServerCertificate=true";

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Username)
            .IsUnique(true);
    }

}