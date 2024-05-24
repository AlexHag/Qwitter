using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Ledger.Contract.Transactions;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.Transactions.Services;

namespace Qwitter.Ledger.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase, ITransactionsController
{
    private readonly IMapper _mapper;
    private readonly ITransactionService _transactionService;

    public TransactionsController(IMapper mapper, ITransactionService transactionService)
    {
        _mapper = mapper;
        _transactionService = transactionService;
    }

    [HttpPost("credit")]
    public async Task<CreditFundsResponse> CreditFunds(CreditFundsRequest request)
    {
        var response = await _transactionService.CreditFunds(request);
        return _mapper.Map<CreditFundsResponse>(response);
    }
}