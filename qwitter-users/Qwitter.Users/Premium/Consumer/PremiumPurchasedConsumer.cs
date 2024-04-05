using MassTransit;
using Qwitter.Core.Application.Kafka;
using Qwitter.Payments.Contract.Transactions.Events;

namespace Qwitter.Users.Premium.Consumers;

[MessageSuffix("premium")]
public class PremiumPurchasedConsumer : IConsumer<TransactionCompletedEvent>
{
    public Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        Console.WriteLine($"PREMIUM PURCHASED FOR User {context.Message.UserId}, transaction {context.Message.TransactionId}");
        return Task.CompletedTask;
    }
}