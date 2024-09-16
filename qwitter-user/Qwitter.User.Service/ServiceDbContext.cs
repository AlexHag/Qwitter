using Microsoft.EntityFrameworkCore;
using Qwitter.User.Service.User.Models;

namespace Qwitter.User.Service;

public class ServiceDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasKey(p => p.UserId);
    }
}