using Microsoft.EntityFrameworkCore;

namespace Qwitter.BankAccounts.Service;

public class ServiceDbContext : DbContext
{
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}