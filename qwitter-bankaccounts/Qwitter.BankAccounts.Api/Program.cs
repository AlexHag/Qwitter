using Qwitter.BankAccounts.Contract.BankAccounts;
using Qwitter.Core.Application;
using Qwitter.Core.Application.RestApiClient;

namespace Qwitter.BankAccounts.Api;

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
        builder.AddRestApiClient<IBankAccountService>();
        return builder;
    }
}