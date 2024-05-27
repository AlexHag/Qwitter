using Microsoft.AspNetCore.Mvc;
using Qwitter.Ledger.Contract.InternalBankTransfer;
using Qwitter.Ledger.Contract.InternalBankTransfer.Models;
using Qwitter.Ledger.Contract.Transactions.Models;
using Qwitter.Ledger.InternalBankTransfer.Services;

namespace Qwitter.Ledger.InternalBankTransfer;

[ApiController]
[Route("internal-bank-transfer")]
public class InternalBankTransferController : ControllerBase, IInternalBankTransferController
{
    private readonly IInternalBankTransferService _internalBankTransferService;

    public InternalBankTransferController(IInternalBankTransferService internalBankTransferService)
    {
        _internalBankTransferService = internalBankTransferService;
    }

    [HttpPost]
    public async Task<TransactionResponse> Transfer(InternalBankTransferRequest request)
    {
        return await _internalBankTransferService.Transfer(request);
    }
}