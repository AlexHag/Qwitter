using MassTransit;
using Qwitter.Domain.Events;
using Qwitter.Users.Database;

namespace Qwitter.Users.Consumers;

public class PremiumPurchasedConsumer : IConsumer<PremiumPurchasedSuccessfullyEvent>
{
    private readonly ILogger<PremiumPurchasedConsumer> _logger;
    private readonly AppDbContext _dbContext;
    public PremiumPurchasedConsumer(
        AppDbContext dbContext,
        ILogger<PremiumPurchasedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PremiumPurchasedSuccessfullyEvent> context)
    {
        _logger.LogInformation($"Consuming Premium Purchase Event for UserId {context.Message.userId}");
        var user = await _dbContext.Users.FindAsync(context.Message.userId);
        if (user is null)
        {
            _logger.LogCritical($"UserId not found {context.Message.userId} when consuming PremiumPurchasedSuccessfullyEvent");
            return;
        }
        if (user.IsPremium)
        {
            // TODO: refund?
            _logger.LogCritical($"User who purchased premium was already premium");
        }
        user.IsPremium = true;
        await _dbContext.SaveChangesAsync();
    }
}