using Qwitter.Core.Application;

namespace Qwitter.Content;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureContentService()
            .Build()
            .ConfigureApp()
            .Run();
}