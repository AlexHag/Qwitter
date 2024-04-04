using Qwitter.Core.Application;

namespace Qwitter.Followers;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureFollowersService()
            .Build()
            .ConfigureApp()
            .Run();
}