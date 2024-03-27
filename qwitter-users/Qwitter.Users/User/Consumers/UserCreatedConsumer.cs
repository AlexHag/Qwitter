using MassTransit;
using Qwitter.Users.Contract.User.Events;

namespace Qwitter.Users.User.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        Console.WriteLine($"Consuming user created event, {context.Message.UserId}, {context.Message.Email}, {context.Message.Username}");
    }
}