using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Contract.Transactions;

[ApiHost(Host.Port, "transactions")]
public interface ITransactionsController
{
    // Authorize as Service
    [HttpPost("allocate-funds")]
    Task<BankAccountAllocationResponse> AllocateBankAccountFunds(AllocateFundsRequest request);

    // Authorize as Service
    [HttpPost("settle-funds")]
    Task<BankAccountAllocationResponse> SettleBankAccountAllocation(SettleAllocationRequest request);

    // Authorize as User
    [HttpPost("transfer-funds")]
    Task<BankAccountTransaction> TransferFunds(TransferFundsRequest request);
}
