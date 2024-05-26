using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Persistence;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.BankAccount.Models;
using Qwitter.Ledger.Contract.Transactions.Models;

namespace Qwitter.Ledger.Contract.BankAccount;

[ApiHost("5005", "bank-account")]
public interface IBankAccountController
{
    [HttpPost]
    Task<BankAccountResponse> CreateBankAccount(CreateBankAccountRequest request);

    [HttpGet("{bankAccountId}")]
    Task<BankAccountResponse> GetBankAccount(Guid bankAaccountId);

    [HttpPost("{bankAccountId}/transactions")]
    Task<IEnumerable<TransactionResponse>> GetBankAccountTransactions(Guid bankAccountId, PaginationRequest request);

    [HttpGet("user/{userId}")]
    Task<List<BankAccountResponse>> GetUserBankAccounts(Guid userId);

    [HttpPut("user/{userId}/primary-bank-account/{bankAccountId}")]
    Task UpdateUserBankPrimaryAccount(Guid userId, Guid bankAccountId);
}