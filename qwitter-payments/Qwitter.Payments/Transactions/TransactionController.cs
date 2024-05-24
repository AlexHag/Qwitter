using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;
using Qwitter.Payments.Contract.Transactions;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Transactions.Models;
using Qwitter.Payments.Transactions.Services;
using Qwitter.Payments.User.Repositories;

namespace Qwitter.Payments.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase, ITransactionController
{
    private readonly ITransactionService _transactionService;
    private readonly IUserRepository _userRepository;
    
    public TransactionController(
        ITransactionService transactionService,
        IUserRepository userRepository)
    {
        _transactionService = transactionService;
        _userRepository = userRepository;
    }

    [HttpPost("create")]
    public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request)
    {
        var user = await _userRepository.GetById(request.UserId);

        if (user?.UserState != UserState.Verified)
        {
            throw new BadRequestApiException("User is not verified");
        }

        return await _transactionService.CreateTransaction(request);
    }
    
    [HttpPost("sync/{transactionId}")]
    public async Task SyncTransaction(Guid transactionId)
    {
        await _transactionService.SyncTransaction(transactionId);
    }
}
