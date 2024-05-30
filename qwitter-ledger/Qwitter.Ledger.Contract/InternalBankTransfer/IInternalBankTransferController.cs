using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.InternalBankTransfer.Models;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Contract.InternalBankTransfer;

[ApiHost(Host.Port, "internal-bank-transfer")]
public interface IInternalBankTransferController
{
    [HttpPost]
    Task<TransactionResponse> Transfer(InternalBankTransferRequest request);
}