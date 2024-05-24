
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Account.Repositories;
using Qwitter.Ledger.Contract.Account;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.ExchangeRates.Repositories;
using Qwitter.Ledger.Transactions.Models;
using Qwitter.Ledger.Transactions.Repositories;
using Qwitter.Ledger.User.Repositories;

namespace Qwitter.Ledger.Transactions.Services;

public interface ITransactionService
{
    Task<TransactionEntity> CreditFunds(CreditFundsRequest request);
}

public class TransactionService : ITransactionService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IExchangeRateRepository _exchangeRate;
    private readonly ITransactionRepository _ledgerRepository;

    public TransactionService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        IExchangeRateRepository exchangeRate,
        ITransactionRepository ledgerRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _exchangeRate = exchangeRate;
        _ledgerRepository = ledgerRepository;
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

        if (request.AccountId != null)
        {
            accountId = request.AccountId.Value;
        }
        else
        {
            if (user.DefaultAccountId == null)
            {
                throw new BadRequestApiException("User has no default account");
            }

            accountId = user.DefaultAccountId.Value;
        }

        var account = await _accountRepository.GetById(accountId) ?? throw new NotFoundApiException("Account not found");

        if (account.AccountStatus == AccountStatus.Cancelled)
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
            AccountId = account.Id,
            PreviousBalance = account.Balance,
            NewBalance = newBalance,
            SourceCurrency = request.Currency,
            DestinationCurrency = account.Currency,
            SourceAmount = request.Amount,
            DestinationAmount = amount,
            ExchangeRate = rate.Value,
            Fee = 0,
            CreatedAt = DateTime.UtcNow
        };

        await _ledgerRepository.Insert(transaction);

        account.Balance += amount;

        await _accountRepository.Update(account);

        return transaction;
    }
}