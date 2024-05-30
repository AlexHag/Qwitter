using Microsoft.AspNetCore.Mvc;
using Qwitter.Ledger.Contract.Crypto;
using Qwitter.Ledger.Contract.Crypto.Models;
using Qwitter.Ledger.Crypto.Services;

namespace Qwitter.Ledger.Crypto;

[ApiController]
[Route("bank-account")]
public class CryptoController : ControllerBase, ICryptoController
{
    private readonly ICryptoService _cryptoService;

    public CryptoController(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    [HttpPost("crypto-wallet")]
    public async Task<BankCryptoWalletResponse> GetBankAccountCryptoWallet(GetBankAccountCryptoWalletRequest request)
    {
        return await _cryptoService.GetBankAccountCryptoWallet(request);
    }
}