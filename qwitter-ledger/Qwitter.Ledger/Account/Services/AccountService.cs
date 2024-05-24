using MapsterMapper;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.Account.Configuration;
using Qwitter.Ledger.Account.Models;
using Qwitter.Ledger.Account.Repositories;
using Qwitter.Ledger.Contract.Account;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.Transactions.Repositories;
using Qwitter.Ledger.User.Repositories;

namespace Qwitter.Ledger.Account.Services;

public interface IAccountService
{
    Task<AccountResponse> CreateLedgerAccount(CreateLedgerAccountRequest request);
    Task<AccountResponse> GetLedgerAccount(Guid accountId);
    Task<List<AccountResponse>> GetUserLedgerAccounts(Guid userId);
    Task<IEnumerable<TransactionResponse>> GetLedgerAccountTransactions(Guid accountId, PaginationRequest request);
    Task UpdateUserPrimaryAccount(Guid userId, Guid accountId);
}

public class AccountService : IAccountService
{
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly BankConfiguration _bankConfiguration;

    public AccountService(
        ILogger<AccountController> logger,
        IMapper mapper,
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository,
        BankConfiguration bankConfiguration)
    {
        _logger = logger;
        _mapper = mapper;
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
        _bankConfiguration = bankConfiguration;
    }

    public async Task<AccountResponse> CreateLedgerAccount(CreateLedgerAccountRequest request)
    {
        var user = await _userRepository.GetById(request.UserId);

        if (user is null)
        {
            throw new NotFoundApiException("User not found");
        }

        if (user.UserState != UserState.Verified)
        {
            _logger.LogWarning("Cannot create account. UserId {UserId} is not verified", request.UserId);
            throw new BadRequestApiException("User is verified");
        }

        var account = new AccountEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            AccountName = request.AccountName,
            AccountNumber = await GenerateAccountNumber(_bankConfiguration.AccountNumberLength),
            RoutingNumber = _bankConfiguration.DefaultRoutingNumber,
            AccountType = request.AccountType,
            AccountStatus = AccountStatus.Active,
            Balance = 0,
            Currency = request.Currency,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _accountRepository.Insert(account);

        return _mapper.Map<AccountResponse>(account);
    }

    public async Task<AccountResponse> GetLedgerAccount(Guid accountId)
    {
        var account = await _accountRepository.GetById(accountId);
        
        if (account is null)
        {
            throw new NotFoundApiException("Account not found");
        }

        return _mapper.Map<AccountResponse>(account);
    }

    public async Task<List<AccountResponse>> GetUserLedgerAccounts(Guid userId)
    {
        var user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new NotFoundApiException("User not found");
        }

        var accounts = await _accountRepository.GetByUserId(userId);

        var response = accounts.Select(_mapper.Map<AccountResponse>).ToList();

        if (user.PrimaryAccountId != null && user.PrimaryAccountId != Guid.Empty)
        {
            var primaryAccount = response.FirstOrDefault(a => a.Id == user.PrimaryAccountId);

            if (primaryAccount != null)
            {
                primaryAccount.IsPrimary = true;
            }
        }

        return response;
    }

    private static readonly Random random = new();

    public async Task<string> GenerateAccountNumber(int length, int maxDepth = 0)
    {
        if (maxDepth > _bankConfiguration.MaxRecursionDepth)
        {
            _logger.LogError($"Failed to generate unique account number after {maxDepth} attempts");
            throw new Exception("Failed to generate unique account number");
        }

        const string chars = "0123456789";
        var number = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        
        var existingAccount = await _accountRepository.GetByAccountNumber(number);

        if (existingAccount != null)
        {
            return await GenerateAccountNumber(length, maxDepth + 1);
        }

        return number;
    }

    public async Task<IEnumerable<TransactionResponse>> GetLedgerAccountTransactions(Guid accountId, PaginationRequest request)
    {
        var transactions = await _transactionRepository.GetByAccountId(accountId, request);
        return transactions.Select(_mapper.Map<TransactionResponse>);
    }

    public async Task UpdateUserPrimaryAccount(Guid userId, Guid accountId)
    {
        var user = await _userRepository.GetById(userId) ?? throw new NotFoundApiException("User not found");
        var account = await _accountRepository.GetById(accountId) ?? throw new NotFoundApiException("Account not found");
        user.PrimaryAccountId = account.Id;
        await _userRepository.Update(user);
    }
}