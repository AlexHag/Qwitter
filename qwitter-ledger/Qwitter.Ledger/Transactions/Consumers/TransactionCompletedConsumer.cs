
using MassTransit;
using Qwitter.Payments.Contract.Transactions.Events;

namespace Qwitter.Transactions.Consumers;

public class TransactionCompletedConsumer : IConsumer<TransactionCompletedEvent>
{
    public Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        throw new NotImplementedException();
    }
}