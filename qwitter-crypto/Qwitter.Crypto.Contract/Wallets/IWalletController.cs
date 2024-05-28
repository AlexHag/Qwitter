
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Crypto.Contract.Wallets.Models;

namespace Qwitter.Crypto.Contract.Wallets;

[ApiHost("7006", "wallets")]
public interface IWalletController
{
    [HttpPost]
    Task<WalletResponse> CreateWallet(CreateWalletRequest request);
    
    [HttpGet("id/{walletId}")]
    Task<WalletResponse> GetWalletById(Guid walletId);
    
    [HttpPut("sync/{address}")]
    Task<SyncWalletResponse> SyncWallet(string address);
}
