using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Payments.Contract.Transactions;
using Qwitter.Payments.Contract.Transactions.Models;
using Qwitter.Payments.Transactions.Models;
using Qwitter.Payments.Transactions.Services;

namespace Qwitter.Payments.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase, ITransactionController
{
    private readonly ITransactionService _transactionService;
    
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // TODO: Add mTLS Authentication
    // [Authorize]
    [HttpPost("create")]
    public async Task<CreateTransactionResponse> CreateTransaction(CreateTransactionRequest request)
    {
        return await _transactionService.CreateTransaction(request);
    }
    
    [HttpPost("sync/{transactionId}")]
    public async Task SyncTransaction(Guid transactionId)
    {
        await _transactionService.SyncTransaction(transactionId);
    }
}
