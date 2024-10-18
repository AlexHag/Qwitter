
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.BankAccounts.Contract.BankAccounts;
using Qwitter.BankAccounts.Contract.BankAccounts.Models;
using Qwitter.BankAccounts.Service.BankAccounts.Models;
using Qwitter.BankAccounts.Service.BankAccounts.Repositorie;
using Qwitter.BankAccounts.Service.BankAccounts.Services;
using Qwitter.BankAccounts.Service.User.Repositories;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Contract.Accounts;
using Qwitter.Funds.Contract.Accounts.Models;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.BankAccounts.Service.BankAccounts;

[ApiController]
[Route("bank-accounts")]
public class BankAccountService : ControllerBase, IBankAccountService
{
    private readonly IUserRepository _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IAccountService _fundsAccountService;
    private readonly IAccountNumberGenerator _accountNumberGenerator;
    private readonly IMapper _mapper;
    private readonly ILogger<BankAccountService> _logger;

    public BankAccountService(
        IUserRepository userRepository,
        IBankAccountRepository bankAccountRepository,
        IAccountService fundsAccountService,
        IAccountNumberGenerator accountNumberGenerator,
        IMapper mapper,
        ILogger<BankAccountService> logger)
    {
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
        _fundsAccountService = fundsAccountService;
        _accountNumberGenerator = accountNumberGenerator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<BankAccountResponse> CreateBankAccount(CreateBankAccountRequest request)
    {
        var user = await _userRepository.GetById(request.UserId);

        if (user.UserState != UserState.Verified)
        {
            throw new BadRequestApiException("Account not verified");
        }

        var accountNumber = _accountNumberGenerator.GenerateAccountNumber();
        var bankAccountId = Guid.NewGuid();

        _ = await _fundsAccountService.CreateAccount(new CreateAccountRequest
        {
            AccountId = bankAccountId,
            Currency = "USD"
        });

        var bankAccountEntity = new BankAccountEntity
        {
            BankAccountId = bankAccountId,
            UserId = request.UserId,
            AccountNumber = accountNumber,
            Currency = request.Currency,
            AvailableBalance = 0,
            TotalBalance = 0,
            IsDefault = false,
            Created = DateTime.UtcNow
        };

        await _bankAccountRepository.Insert(bankAccountEntity);
        return _mapper.Map<BankAccountResponse>(bankAccountEntity);
    }

    [HttpGet("{userId}")]
    public async Task<List<BankAccountResponse>> GetAccounts(Guid userId)
    {
        var accounts = await _bankAccountRepository.GetAllByUserId(userId);
        return accounts.Select(_mapper.Map<BankAccountResponse>).ToList();
    }

    [HttpPut("set-default")]
    public async Task<BankAccountResponse> SetDefaultBankAccount(SetDefaultBankAccountRequest request)
    {
        var bankAccount = await _bankAccountRepository.GetById(request.BankAccountId);

        if (bankAccount.UserId != request.UserId)
        {
            throw new BadRequestApiException("Bank account does not belong to user");
        }

        var currentDefaultAccount = await _bankAccountRepository.TryGetDefaultByUserId(request.UserId);

        if (currentDefaultAccount != null)
        {
            currentDefaultAccount.IsDefault = false;
            await _bankAccountRepository.Update(currentDefaultAccount);
        }

        bankAccount.IsDefault = true;

        await _bankAccountRepository.Update(bankAccount);
        return _mapper.Map<BankAccountResponse>(bankAccount);
    }
}