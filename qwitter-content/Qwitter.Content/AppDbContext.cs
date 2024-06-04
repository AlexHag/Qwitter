using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Comments.Models;
using Qwitter.Content.Posts.Models;
using Qwitter.Content.Users.Models;

namespace Qwitter.Content;

public class AppDbContext : DbContext
{
    public DbSet<PostEntity> Posts { get; set; }
    // public DbSet<CommentEntity> Comments { get; set; }
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

        modelBuilder.Entity<PostEntity>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId);
        
        modelBuilder.Entity<UserEntity>()
            .HasKey(u => u.UserId);
    }
}
