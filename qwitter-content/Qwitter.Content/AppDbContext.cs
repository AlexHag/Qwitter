using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Posts.Models;
using Qwitter.Content.Users.Models;

namespace Qwitter.Content;

public class AppDbContext : DbContext
{
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostEntity>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PostEntity>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);
        
        modelBuilder.Entity<UserEntity>()
            .HasKey(u => u.UserId);
    }
}
