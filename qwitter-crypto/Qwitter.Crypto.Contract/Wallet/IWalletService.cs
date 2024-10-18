using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Crypto.Contract.Wallet.Models;

namespace Qwitter.Crypto.Contract.Wallet;

[ApiHost(Host.Name, "wallet")]
public interface IWalletService
{
    [HttpPost]
    Task<WalletResponse> CreateWallet(CreateWalletRequest request);
    
    [HttpGet("id/{walletId}")]
    Task<WalletResponse> GetWalletById(Guid walletId);
    
    [HttpPut("sync/{address}")]
    Task<SyncWalletResponse> SyncWallet(string address);
}
