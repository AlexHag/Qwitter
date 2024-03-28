using Qwitter.Core.Application;

namespace Qwitter.Social;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureSocialService()
            .Build()
            .ConfigureApp()
            .Run();
}