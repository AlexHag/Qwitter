using Qwitter.Core.Application;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.User.Contract.Auth;
using Qwitter.User.Contract.User;

namespace Qwitter.Users.Api;

public static class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureServices()
            .Build()
            .ConfigureApp()
            .Run();

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddRestApiClientV2<IAuthService>();
        builder.AddRestApiClientV2<IUserService>();

        return builder;
    }
}