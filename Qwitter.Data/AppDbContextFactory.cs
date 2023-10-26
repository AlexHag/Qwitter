using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Qwitter.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = args.Any() ? args[0] : "Server=localhost, 1433;Database=Qwitter;User Id=SA;Password=qwitter_password_xyz@secure123;TrustServerCertificate=true;";

        builder.UseSqlServer(connectionString, db => db.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name));
        return new AppDbContext(builder.Options);
    }
}