
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Ledger.Contract.Crypto.Models;

namespace Qwitter.Ledger.Contract.Crypto;

[ApiHost(Host.Port, "bank-account")]
public interface ICryptoController
{
    [HttpPost("crypto-wallet")]
    Task<BankCryptoWalletResponse> GetBankAccountCryptoWallet(GetBankAccountCryptoWalletRequest request);
}