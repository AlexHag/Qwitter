using MapsterMapper;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.BankAccount.Configuration;
using Qwitter.Ledger.BankAccount.Models;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.Contract.Account;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.Transactions.Repositories;
using Qwitter.Ledger.User.Repositories;

namespace Qwitter.Ledger.BankAccount.Services;

public interface IBankAccountService
{
    Task<BankAccountResponse> CreateBankAccount(CreateBankAccountRequest request);
    Task<BankAccountResponse> GetBankAccount(Guid bankAccountId);
    Task<List<BankAccountResponse>> GetUserBankAccounts(Guid userId);
    Task<IEnumerable<TransactionResponse>> GetBankAccountTransactions(Guid bankAccountId, PaginationRequest request);
    Task UpdateUserPrimaryBankAccount(Guid userId, Guid bankAccountId);
}

public class BankAccountService : IBankAccountService
{
    private readonly ILogger<BankAccountService> _logger;
    private readonly IMapper _mapper;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly BankConfiguration _bankConfiguration;

    public BankAccountService(
        ILogger<BankAccountService> logger,
        IMapper mapper,
        IBankAccountRepository bankAccountRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository,
        BankConfiguration bankConfiguration)
    {
        _logger = logger;
        _mapper = mapper;
        _bankAccountRepository = bankAccountRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
        _bankConfiguration = bankConfiguration;
    }

    public async Task<BankAccountResponse> CreateBankAccount(CreateBankAccountRequest request)
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

        var bankAccount = new BankAccountEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            AccountName = request.AccountName,
            AccountNumber = await GenerateAccountNumber(_bankConfiguration.AccountNumberLength),
            RoutingNumber = _bankConfiguration.DefaultRoutingNumber,
            AccountType = request.AccountType,
            AccountStatus = BankAccountStatus.Active,
            Balance = 0,
            Currency = request.Currency,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _bankAccountRepository.Insert(bankAccount);

        return _mapper.Map<BankAccountResponse>(bankAccount);
    }

    public async Task<BankAccountResponse> GetBankAccount(Guid bankAccountId)
    {
        var bankAccount = await _bankAccountRepository.GetById(bankAccountId);
        
        if (bankAccount is null)
        {
            throw new NotFoundApiException("Account not found");
        }

        return _mapper.Map<BankAccountResponse>(bankAccount);
    }

    public async Task<List<BankAccountResponse>> GetUserBankAccounts(Guid userId)
    {
        var user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new NotFoundApiException("User not found");
        }

        var bankAccounts = await _bankAccountRepository.GetAllByUserId(userId);

        var response = bankAccounts.Select(_mapper.Map<BankAccountResponse>).ToList();

        if (user.PrimaryBankAccountId != null && user.PrimaryBankAccountId != Guid.Empty)
        {
            var primaryBankAccount = response.FirstOrDefault(a => a.Id == user.PrimaryBankAccountId);

            if (primaryBankAccount != null)
            {
                primaryBankAccount.IsPrimary = true;
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
        
        var existingBankAccount = await _bankAccountRepository.GetByAccountNumber(number);

        if (existingBankAccount != null)
        {
            return await GenerateAccountNumber(length, maxDepth + 1);
        }

        return number;
    }

    public async Task<IEnumerable<TransactionResponse>> GetBankAccountTransactions(Guid bankAccountId, PaginationRequest request)
    {
        var transactions = await _transactionRepository.GetByBankAccountId(bankAccountId, request);
        return transactions.Select(_mapper.Map<TransactionResponse>);
    }

    public async Task UpdateUserPrimaryBankAccount(Guid userId, Guid bankAccountId)
    {
        var user = await _userRepository.GetById(userId) ?? throw new NotFoundApiException("User not found");
        var bankAccount = await _bankAccountRepository.GetById(bankAccountId) ?? throw new NotFoundApiException("Account not found");
        user.PrimaryBankAccountId = bankAccount.Id;
        await _userRepository.Update(user);
    }
}