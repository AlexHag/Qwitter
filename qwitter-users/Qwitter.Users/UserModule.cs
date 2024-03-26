using Microsoft.EntityFrameworkCore;
using Qwitter.Users.Auth.Services;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users;

public static class UserModule
{
    public static WebApplicationBuilder ConfigureUserService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        return builder;
    }
}