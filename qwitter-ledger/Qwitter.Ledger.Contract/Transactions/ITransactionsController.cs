using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Contract.Transactions;

[ApiHost("5005", "transactions")]
public interface ITransactionsController
{
    [HttpPost("credit")]
    Task<CreditFundsResponse> CreditFunds(CreditFundsRequest request);
    // Task DebitFunds();
    // Task ReserveFunds();
    // Task DebitReservedFunds();
    // Task CancelReservedFunds();
}
