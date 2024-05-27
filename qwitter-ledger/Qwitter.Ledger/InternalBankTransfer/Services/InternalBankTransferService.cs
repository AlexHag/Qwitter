using MapsterMapper;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;
using Qwitter.Ledger.BankAccount.Repositories;
using Qwitter.Ledger.Contract.BankAccount.Models;
using Qwitter.Ledger.Contract.InternalBankTransfer.Models;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.Transactions.Services;
using Qwitter.Ledger.User.Repositories;

namespace Qwitter.Ledger.InternalBankTransfer.Services;

public interface IInternalBankTransferService
{
    Task<TransactionResponse> Transfer(InternalBankTransferRequest request);
}

public class InternalBankTransferService : IInternalBankTransferService
{
    private readonly ILogger<InternalBankTransferService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionService _transactionService;

    public InternalBankTransferService(
        ILogger<InternalBankTransferService> logger,
        IMapper mapper,
        IUserRepository userRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionService transactionService)
    {
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionService = transactionService;
    }

    public async Task<TransactionResponse> Transfer(InternalBankTransferRequest request)
    {
        var fromAccount = await _bankAccountRepository.GetById(request.FromAccountId) ?? throw new NotFoundApiException("Bank account not found");

        if (fromAccount.UserId != request.UserId)
        {
            throw new BadRequestApiException("User does not own the bank account");
        }

        if (fromAccount.Balance < request.Amount)
        {
            throw new BadRequestApiException("Insufficient funds");
        }

        var toAccount = await _bankAccountRepository.GetById(request.ToAccountId) ?? throw new NotFoundApiException("Bank account not found");

        if (toAccount.AccountStatus != BankAccountStatus.Active)
        {
            throw new BadRequestApiException("Cannot transfer funds to that account");
        }

        if (fromAccount.UserId != toAccount.UserId)
        {
            var toUser = await _userRepository.GetById(toAccount.UserId) ?? throw new NotFoundApiException("User not found");

            if (toUser.UserState != UserState.Verified)
            {
                throw new BadRequestApiException("Cannot transfer funds to that user");
            }
        }

        var debitFundsRequest = new DebitFundsRequest
        {
            UserId = request.UserId,
            BankAccountId = request.FromAccountId,
            Amount = request.Amount,
            Currency = fromAccount.Currency
        };

        // TODO: Make transactions preauth and commit once both are successful
        var transaction = await _transactionService.DebitFunds(debitFundsRequest);

        var creditFundsRequest = new CreditFundsRequest
        {
            UserId = toAccount.UserId,
            BankAccountId = toAccount.Id,
            Amount = transaction.DestinationAmount,
            Currency = fromAccount.Currency
        };

        await _transactionService.CreditFunds(creditFundsRequest);

        return _mapper.Map<TransactionResponse>(transaction);
    }
}