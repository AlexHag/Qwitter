using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Kafka;
using Qwitter.Users.Follows.Repositories;
using Qwitter.Users.Premium.Consumers;
using Qwitter.Users.Repositories.User;
using Qwitter.Users.User.Consumers;

namespace Qwitter.Users;

public static class UserModule
{
    public static WebApplicationBuilder ConfigureUserService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IFollowsRepository, FollowersRepository>();
        builder.Services.AddScoped<IMapper, Mapper>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.RegisterConsumer<UserCreatedConsumer>("user-group");
        builder.RegisterConsumer<PremiumPurchasedConsumer>("user-group");
        builder.RegisterConsumer<UserStartedFollowingConsumer>("user-group");
        builder.RegisterConsumer<UserStoppedFollowingConsumer>("user-group");

        builder.UseKafka();

        return builder;
    }
}