using Microsoft.EntityFrameworkCore;

namespace Qwitter.Followers;

public class AppDbContext : DbContext
{
    // public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<UserEntity>()
    //         .HasKey(p => p.UserId);

    //     modelBuilder.Entity<UserEntity>()
    //         .HasIndex(p => p.Username)
    //         .IsUnique();
    // }
}