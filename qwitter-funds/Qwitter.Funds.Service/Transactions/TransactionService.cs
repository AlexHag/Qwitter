using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Funds.Contract.Transactions;
using Qwitter.Funds.Contract.Transactions.Models;
using Qwitter.Funds.Service.Transactions.Repositories;

namespace Qwitter.Funds.Service.Transactions;

[ApiController]
[Route("transactions")]
public class TransactionService : ControllerBase, ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    [HttpPost("filter")]
    public async Task<PaginationResponse<TransactionResponse>> GetTransactions(PaginationRequest<TransactionFilterRequest> request)
    {
        if (request.Filter == null)
        {
            return new() { Count = 0, Items = [] };
        }

        var transactions = await _transactionRepository.GetByAccountId(request.Filter.AccountId, request.Skip, request.Take);
        var transactionResponse = _mapper.Map<List<TransactionResponse>>(transactions);

        return new() { Count = transactionResponse.Count, Items = transactionResponse };
    }
}
