
using Qwitter.Funds.Service.CurrencyExchange.Models;
using Qwitter.Funds.Service.CurrencyExchange.Repositories;
using Qwitter.Funds.Service.ExchangeRate.Repositories;

namespace Qwitter.Funds.Service.CurrencyExchange;

public interface ICurrencyExchangeActions
{
    Task<CurrencyExchangeEntity> Exchange(ExchangeCurrencyCommand command);
}

public class CurrencyExchangeActions : ICurrencyExchangeActions
{
    private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly ICurrencyAccountRepository _currencyAccountRepository;

    public CurrencyExchangeActions(
        ICurrencyExchangeRepository currencyExchangeRepository,
        IExchangeRateRepository exchangeRateRepository,
        ICurrencyAccountRepository currencyAccountRepository)
    {
        _currencyExchangeRepository = currencyExchangeRepository;
        _exchangeRateRepository = exchangeRateRepository;
        _currencyAccountRepository = currencyAccountRepository;
    }

    public async Task<CurrencyExchangeEntity> Exchange(ExchangeCurrencyCommand command)
    {
        var sourceAccount = await _currencyAccountRepository.GetByCurrency(command.SourceCurrency);

        if (sourceAccount.Balance < command.Amount)
        {
            throw new Exception($"Unable to exchange {command.Amount} {command.SourceCurrency} to {command.DestinationCurrency}. Insufficient funds in the system.");
        }

        var rate = await _exchangeRateRepository.GetLatestByCurrencyPair(command.SourceCurrency, command.DestinationCurrency);
        var destinationAccount = await _currencyAccountRepository.GetByCurrency(command.DestinationCurrency);

        var destinationAmount = command.Amount * rate.Rate;

        sourceAccount.Balance += command.Amount;
        destinationAccount.Balance -= destinationAmount;

        var exchangeEntity = new CurrencyExchangeEntity
        {
            CurrencyExchangeId = Guid.NewGuid(),
            SourceCurrency = command.SourceCurrency,
            DestinationCurrency = command.DestinationCurrency,
            SourceAmount = command.Amount,
            DestinationAmount = destinationAmount,
            ExchangeRateId = rate.ExchangeRateId,
            Rate = rate.Rate
        };

        // TODO: Make this into a db transaction
        await _currencyExchangeRepository.Insert(exchangeEntity);
        await _currencyAccountRepository.Update(sourceAccount);
        await _currencyAccountRepository.Update(destinationAccount);

        return exchangeEntity;
    }
}