using MassTransit;
using Qwitter.Domain.Events;
using Qwitter.Social.Database;
using Microsoft.EntityFrameworkCore;

namespace Qwitter.Social.Consumers;

public class PremiumPurchasedConsumer : IConsumer<PremiumPurchasedSuccessfullyEvent>
{
    private readonly ILogger<PremiumPurchasedConsumer> _logger;
    private readonly AppDbContext _dbContext;

    public PremiumPurchasedConsumer(
        ILogger<PremiumPurchasedConsumer> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<PremiumPurchasedSuccessfullyEvent> context)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"User bought premium, updating users content");

        var userPosts = await _dbContext.Posts.Where(p => p.UserId == context.Message.userId).ToListAsync();
        foreach (var post in userPosts)
        {
            post.IsPremium = true;
            post.UpdatedAt = now;
        }

        var userComments = await _dbContext.Comments.Where(p => p.UserId == context.Message.userId).ToListAsync();
        foreach (var comment in userComments)
        {
            comment.IsPremium = true;
            comment.UpdatedAt = now;
        }

        await _dbContext.SaveChangesAsync();
    }
}