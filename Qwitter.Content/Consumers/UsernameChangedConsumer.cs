using MassTransit;
using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Database;
using Qwitter.Domain.DTO;

namespace Qwitter.Content.Consumers;

public class UsernameChangedConsumer : IConsumer<UpdateUsernameDTO>
{
    private readonly ILogger<UsernameChangedConsumer> _logger;
    private readonly AppDbContext _dbContext;

    public UsernameChangedConsumer(
        ILogger<UsernameChangedConsumer> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<UpdateUsernameDTO> context)
    {
        var now = DateTime.UtcNow;
        _logger.LogInformation($"User changed username");

        var userPosts = await _dbContext.Posts.Where(p => p.UserId == context.Message.UserId).ToListAsync();
        foreach (var post in userPosts)
        {
            post.Username = context.Message.NewUsername;
            post.UpdatedAt = now;
        }

        var userComments = await _dbContext.Comments.Where(p => p.UserId == context.Message.UserId).ToListAsync();
        foreach (var comment in userComments)
        {
            comment.Username = context.Message.NewUsername;
            comment.UpdatedAt = now;
        }

        await _dbContext.SaveChangesAsync();
    }
}