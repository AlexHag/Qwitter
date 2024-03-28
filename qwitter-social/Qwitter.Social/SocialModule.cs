using MapsterMapper;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Users.Contract.User;

namespace Qwitter.Social;

public static class SocialModule
{
    public static WebApplicationBuilder ConfigureSocialService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMapper, Mapper>();

        builder.AddRestApiClient<IUserController>();

        builder.UseKafka();

        return builder;
    }
}