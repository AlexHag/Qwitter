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

    [HttpPost("allocate-funds")]
    public async Task<BankAccountAllocationResponse> AllocateBankAccountFunds(AllocateFundsRequest request)
    {
        var (allocation, transaction) = await _transactionService.AllocateBankAccountFunds(request);

        return new()
        {
            Allocation = _mapper.Map<FundAllocation>(allocation),
            Transaction = _mapper.Map<BankAccountTransaction>(transaction)
        };
    }

    [HttpPost("settle-funds")]
    public async Task<BankAccountAllocationResponse> SettleBankAccountAllocation(SettleAllocationRequest request)
    {
        var (allocation, transaction) = await _transactionService.SettleBankAccountAllocation(request);

        return new()
        {
            Allocation = _mapper.Map<FundAllocation>(allocation),
            Transaction = _mapper.Map<BankAccountTransaction>(transaction)
        };
    }

    [HttpPost("transfer-funds")]
    public async Task<BankAccountTransaction> TransferFunds(TransferFundsRequest request)
    {
        var transaction = await _transactionService.TransferFunds(request);
        return _mapper.Map<BankAccountTransaction>(transaction);
    }
}