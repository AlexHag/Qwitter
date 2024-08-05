using MassTransit;
using Qwitter.Users.Contract.User.Events;

namespace Qwitter.Users.User.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        _logger.LogInformation("Consuming user created event, {userId}, {email}, {username}", context.Message.UserId, context.Message.Email, context.Message.Username);
        return Task.CompletedTask;
    }
}