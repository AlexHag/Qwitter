using Qwitter.Exchange.Service.CurrencyAccounts.Models;
using Qwitter.Exchange.Service.CurrencyAccounts.Repositories;

namespace Qwitter.Exchange.Service.CurrencyAccounts;

public interface ICurrencyAccountService
{
    Task<CurrencyAccountEntity> GetSourceCurrencyAccount(string currency);
    Task<CurrencyAccountEntity> GetDestinationCurrencyAccount(string currency);
}

public class CurrencyAccountService : ICurrencyAccountService
{
    private readonly ICurrencyAccountRepository _currencyAccountRepository;

    public CurrencyAccountService(ICurrencyAccountRepository currencyAccountRepository)
    {
        _currencyAccountRepository = currencyAccountRepository;
    }

    public async Task<CurrencyAccountEntity> GetDestinationCurrencyAccount(string currency)
        => await _currencyAccountRepository.GetByCurrency(currency);

    public async Task<CurrencyAccountEntity> GetSourceCurrencyAccount(string currency)
        => await _currencyAccountRepository.GetByCurrency(currency);
}