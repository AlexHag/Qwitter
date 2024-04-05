using MassTransit;
using Qwitter.Payments.Contract.Transactions.Events;

namespace Qwitter.Payments.Transactions.Consumers;

public class TransactionCreatedConsumer : IConsumer<TransactionCreatedEvent>
{
    public Task Consume(ConsumeContext<TransactionCreatedEvent> context)
    {
        Console.WriteLine("Transaction created");
        return Task.CompletedTask;
    }
}
