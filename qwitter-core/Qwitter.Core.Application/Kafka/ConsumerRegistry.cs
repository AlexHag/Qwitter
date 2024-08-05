namespace Qwitter.Core.Application.Kafka;

public interface IConsumerRegistry
{
    void RegisterConsumer(ConsumerRegistration consumerRegistration);
    IEnumerable<ConsumerRegistration> GetRegistrations();
}

public class ConsumerRegistry : IConsumerRegistry
{
    private List<ConsumerRegistration> _consumers = [];

    public void RegisterConsumer(ConsumerRegistration consumerRegistration)
    {
        _consumers.Add(consumerRegistration);
    }

    public IEnumerable<ConsumerRegistration> GetRegistrations()
    {
        return _consumers;
    }
}
