using Qwitter.Core.Application;

namespace Qwitter.Crypto;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureCryptoModule()
            .Build()
            .ConfigureApp()
            .Run();
}