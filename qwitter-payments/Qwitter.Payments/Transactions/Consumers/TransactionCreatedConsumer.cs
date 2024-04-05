using MassTransit;
using Qwitter.Payments.Contract.Transactions.Events;

namespace Qwitter.Payments.Transactions.Consumers;

public class TransactionCreatedConsumer : IConsumer<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedConsumer> _logger;

    public TransactionCreatedConsumer(ILogger<TransactionCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TransactionCreatedEvent> context)
    {
        _logger.LogInformation("Consuming transaction created event, {TransactionId}", context.Message.TransactionId);
        return Task.CompletedTask;
    }
}
