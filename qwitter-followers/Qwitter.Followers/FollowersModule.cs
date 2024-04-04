using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Users.Contract.User;

namespace Qwitter.Followers;

public static class FollowersModule
{
    public static WebApplicationBuilder ConfigureFollowersService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMapper, Mapper>();

        builder.AddRestApiClient<IUserController>();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

        builder.UseKafka();

        return builder;
    }
}