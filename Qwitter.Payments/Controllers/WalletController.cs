using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qwitter.Payments.Database;
using Qwitter.Payments.Service;
using Qwitter.Domain.DTO;
using Qwitter.Payments.Entities;

namespace Qwitter.Payments.Controllers;

[ApiController]
[Route("wallet")]
public class WalletController : ControllerBase
{
    private readonly ILogger<WalletController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly INethereumService _nethereumService;

    public WalletController(
        ILogger<WalletController> logger,
        AppDbContext dbContext,
        INethereumService nethereumService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _nethereumService = nethereumService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserWallet(Guid userId)
    {
        var wallet = await _dbContext.UserWallets.Where(p => p.UserId == userId).FirstOrDefaultAsync();
        if (wallet is not null)
        {
            var balance = await _nethereumService.CheckBalance(wallet.Address);
            if (balance != wallet.Balance)
            {
                wallet.Balance = balance;
                await _dbContext.SaveChangesAsync();
            }

            return Ok(new UserWalletDTO
            {
                Address = wallet.Address,
                Balance = wallet.Balance
            });
        }

        var privateMnemonic = _nethereumService.GeneratePrivateMnemonic();
        var address = _nethereumService.GetAddressFromPrivateMnemonic(privateMnemonic);
        
        var userWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PrivateMnemonic = privateMnemonic,
            Address = address,
            Balance = 0
        };

        await _dbContext.UserWallets.AddAsync(userWallet);
        await _dbContext.SaveChangesAsync();

        return Ok(new UserWalletDTO
        {
            Address = userWallet.Address,
            Balance = userWallet.Balance
        });
    }
}
