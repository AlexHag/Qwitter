using Qwitter.Core.Application;

namespace Qwitter.Users;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureUserService()
            .Build()
            .ConfigureApp()
            .Run();
}