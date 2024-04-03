using Qwitter.Core.Application;

namespace Qwitter.Payments;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigurePaymentsModule()
            .Build()
            .ConfigureApp()
            .Run();
}