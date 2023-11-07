using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qwitter.Payments.Database;
using Qwitter.Payments.Service;
using Qwitter.Domain.Events;
using Qwitter.Domain.Api;
using Qwitter.Domain.DTO;

namespace Qwitter.Payments.Controllers;

[ApiController]
[Route("payment")]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private readonly IUserClient _userClient;
    private readonly INethereumService _nethereumService;
    private readonly ITopicProducer<PremiumPurchaseRequestedEvent> _premiumPurchaseRequestEventProducer;

    public PaymentController(
        ILogger<PaymentController> logger,
        IConfiguration configuration,
        AppDbContext dbContext,
        IUserClient userClient,
        INethereumService nethereumService,
        ITopicProducer<PremiumPurchaseRequestedEvent> premiumPurchaseRequestEventProducer)
    {
        _logger = logger;
        _configuration = configuration;
        _dbContext = dbContext;
        _userClient = userClient;
        _nethereumService = nethereumService;
        _premiumPurchaseRequestEventProducer = premiumPurchaseRequestEventProducer;
    }

    [HttpPost]
    [Route("premium/{userId}")]
    public async Task<IActionResult> BuyPremium(Guid userId)
    {
        var wallet = await _dbContext.UserWallets.Where(p => p.UserId == userId).FirstOrDefaultAsync();
        if (wallet is null)
        {
            _logger.LogWarning($"User without wallet tried to buy premium. UserId: {userId}");
            return NotFound("No wallet found for user");
        }

        var user = await _userClient.GetUser(userId);
        if (user is null)
        {
            _logger.LogCritical($"User with wallet tried to buy premium but user was not found from user service. UserId: {userId}");
            return NotFound("Could not find user");
        }

        if (user.IsPremium)
        {
            _logger.LogWarning("User is already premium but tried to buy it again");
            return Accepted("Premium already purchased");
        }

        var premiumPrice = Decimal.Parse(_configuration["Premium:QwitterPremiumPrice"]!);

        var balance = await _nethereumService.CheckBalance(wallet.Address);
        if (balance < premiumPrice)
        {
            return BadRequest($"Insufficient balance. Price: {premiumPrice}, Balance: {balance}");
        }

        if (balance != wallet.Balance)
        {
            wallet.Balance = balance;
            await _dbContext.SaveChangesAsync();
        }

        await _premiumPurchaseRequestEventProducer.Produce(new PremiumPurchaseRequestedEvent
        (
            wallet.Id,
            DateTime.UtcNow
        ), HttpContext.RequestAborted);

        return Accepted();
    } 
}
