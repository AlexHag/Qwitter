using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Contract.Transactions;

[ApiHost(Host.Port, "transactions")]
public interface ITransactionsController
{
    [HttpPost("credit")]
    Task<TransactionResponse> CreditFunds(CreditFundsRequest request);
    [HttpPost("debit")]
    Task<TransactionResponse> DebitFunds(DebitFundsRequest request);
    // Task ReserveFunds();
    // Task DebitReservedFunds();
    // Task CancelReservedFunds();
}
