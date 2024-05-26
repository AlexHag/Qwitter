
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.Contract.BankAccount.Models;
using Qwitter.Ledger.Contract.Transactions.Events;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.Transactions.Models;
using Qwitter.Ledger.Transactions.Repositories;
using Qwitter.Ledger.User.Repositories;

namespace Qwitter.Ledger.Transactions.Services;

public interface ITransactionService
{
    Task<TransactionEntity> CreditFunds(CreditFundsRequest request);
    Task<TransactionEntity> DebitFunds(DebitFundsRequest request);
}

public class TransactionService : ITransactionService
{
    private readonly IUserRepository _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IExchangeRateRepository _exchangeRate;
    private readonly ITransactionRepository _ledgerRepository;
    private readonly IEventProducer _eventProducer;

    public TransactionService(
        IUserRepository userRepository,
        IBankAccountRepository bankAccountRepository,
        IExchangeRateRepository exchangeRate,
        ITransactionRepository ledgerRepository,
        IEventProducer eventProducer)
    {
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
        _exchangeRate = exchangeRate;
        _ledgerRepository = ledgerRepository;
        _eventProducer = eventProducer;
    }

    public async Task<TransactionEntity> CreditFunds(CreditFundsRequest request)
    {
        var user = await _userRepository.GetById(request.UserId) ?? throw new NotFoundApiException("User not found");

        if (user.UserState != UserState.Verified)
        {
            throw new BadRequestApiException("User is not verified");
        }

        if (request.Amount < 0)
        {
            throw new BadRequestApiException("Amount must be greater than 0");
        }

        Guid accountId;

        if (request.BankAccountId != null)
        {
            accountId = request.BankAccountId.Value;
        }
        else
        {
            if (user.PrimaryBankAccountId == null)
            {
                throw new BadRequestApiException("User has no default account");
            }

            accountId = user.PrimaryBankAccountId.Value;
        }

        var account = await _bankAccountRepository.GetById(accountId) ?? throw new NotFoundApiException("Account not found");

        if (account.AccountStatus == BankAccountStatus.Cancelled)
        {
            throw new BadRequestApiException("Account is cancelled");
        }

        var rate = await _exchangeRate.GetExchangeRate(request.Currency, account.Currency);

        if (rate is null)
        {
            throw new BadRequestApiException("Exchange rate not found");
        }

        var amount = request.Amount * rate.Value;
        var newBalance = account.Balance + amount;
    
        var transaction = new TransactionEntity
        {
            Id = Guid.NewGuid(),
            BankAccountId = account.Id,
            PreviousBalance = account.Balance,
            NewBalance = newBalance,
            SourceCurrency = request.Currency,
            DestinationCurrency = account.Currency,
            SourceAmount = request.Amount,
            DestinationAmount = amount,
            ExchangeRate = rate.Value,
            Fee = 0,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _ledgerRepository.Insert(transaction);

        account.Balance += amount;

        await _bankAccountRepository.Update(account);

        return transaction;
    }

    public async Task<TransactionEntity> DebitFunds(DebitFundsRequest request)
    {
        var user = await _userRepository.GetById(request.UserId) ?? throw new NotFoundApiException("User not found");

        if (user.UserState != UserState.Verified)
        {
            throw new BadRequestApiException("User is not verified");
        }

        if (request.Amount < 0)
        {
            throw new BadRequestApiException("Amount must be greater than 0");
        }

        Guid accountId;

        if (request.BankAccountId != null)
        {
            accountId = request.BankAccountId.Value;
        }
        else
        {
            if (user.PrimaryBankAccountId == null)
            {
                throw new BadRequestApiException("User has no default account");
            }

            accountId = user.PrimaryBankAccountId.Value;
        }

        var account = await _bankAccountRepository.GetById(accountId) ?? throw new NotFoundApiException("Account not found");

        if (account.AccountStatus == BankAccountStatus.Cancelled)
        {
            throw new BadRequestApiException("Account is cancelled");
        }

        if (account.AccountStatus == BankAccountStatus.Frozen)
        {
            throw new BadRequestApiException("Account is frozen");
        }

        var currency = request.Currency ?? account.Currency;

        var rate = await _exchangeRate.GetExchangeRate(currency, account.Currency);

        if (rate is null)
        {
            throw new BadRequestApiException("Exchange rate not found");
        }

        var amount = request.Amount * rate.Value;

        if (amount > account.Balance && !account.OverdraftAllowed)
        {
            throw new BadRequestApiException("Insufficient funds");
        }

        var newBalance = account.Balance - amount;
    
        var transaction = new TransactionEntity
        {
            Id = Guid.NewGuid(),
            BankAccountId = account.Id,
            PreviousBalance = account.Balance,
            NewBalance = newBalance,
            SourceCurrency = account.Currency,
            DestinationCurrency = currency,
            SourceAmount = amount,
            DestinationAmount = request.Amount,
            ExchangeRate = rate.Value,
            Fee = 0,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _ledgerRepository.Insert(transaction);

        if (amount > account.Balance)
        {
            await _eventProducer.Produce(new TransactionOverdraftEvent
            {
                UserId = user.UserId,
                AccountId = account.Id,
                TransactionId = transaction.Id
            });
        }

        account.Balance -= amount;
        await _bankAccountRepository.Update(account);

        return transaction;
    }
}