using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Crypto.Contract.Wallets;
using Qwitter.Crypto.Contract.Wallets.Models;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Wallets.Repositories;
using Qwitter.Crypto.Wallets.Services;

namespace Qwitter.Crypto.Wallets;

[ApiController]
[Route("wallets")]
public class WalletController : ControllerBase, IWalletController
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;;
    }

    [HttpPost]
    public async Task<WalletResponse> CreateWallet(CreateWalletRequest request)
    {
        return await _walletService.CreateWallet(request);
    }

    [HttpGet("id/{walletId}")]
    public async Task<WalletResponse> GetWalletById(Guid walletId)
    {
        return await _walletService.GetWalletById(walletId);
    }

    [HttpPut("sync/{address}")]
    public async Task<SyncWalletResponse> SyncWallet(string address)
    {
        return await _walletService.SyncWallet(address);
    }
}