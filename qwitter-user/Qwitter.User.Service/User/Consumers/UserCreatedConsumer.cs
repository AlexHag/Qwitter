using MassTransit;
using Qwitter.User.Contract.Events;

namespace Qwitter.User.Service.User.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        Console.WriteLine($"Consumed message: {context.Message.UserId}");
        return Task.CompletedTask;
    }
}