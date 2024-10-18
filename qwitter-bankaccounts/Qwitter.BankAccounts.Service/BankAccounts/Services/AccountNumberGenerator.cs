
namespace Qwitter.BankAccounts.Service.BankAccounts.Services;

public interface IAccountNumberGenerator
{
    string GenerateAccountNumber();
}

public class AccountNumberGenerator : IAccountNumberGenerator
{
    private static readonly Random random = new();
    private const int length = 10;

    public string GenerateAccountNumber()
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}