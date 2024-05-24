using Qwitter.Core.Application;

namespace Qwitter.Ledger;

public class Program
{
    public static void Main(string[] args)
        => WebApplication.CreateBuilder(args)
            .ConfigureBuilder()
            .ConfigureLedgerModule()
            .Build()
            .ConfigureApp()
            .Run();
}