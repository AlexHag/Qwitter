using System.Reflection;
using System.Text.Json;
using Confluent.Kafka;

namespace Qwitter.Core.Application.Kafka;

public interface IEventProducer
{
    Task Produce(object @event);
    Task Produce(object @event, string? topicSuffix);
}

public class EventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;

    public EventProducer(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task Produce(object @event)
    {
        var messageAttribute = @event.GetType().GetCustomAttribute<MessageAttribute>();

        if (messageAttribute is null || string.IsNullOrEmpty(messageAttribute.TopicName))
            throw new InvalidOperationException($"Event {@event.GetType()} must have a MessageAttribute with a topic name");

        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event)
        };

        await _producer.ProduceAsync(messageAttribute.TopicName, message);
    }

    public async Task Produce(object @event, string? topicSuffix)
    {
        var messageAttribute = @event.GetType().GetCustomAttribute<MessageAttribute>();

        if (messageAttribute is null || string.IsNullOrEmpty(messageAttribute.TopicName))
            throw new InvalidOperationException($"Event {@event.GetType()} must have a MessageAttribute with a topic name");

        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event)
        };

        string topic = string.IsNullOrWhiteSpace(topicSuffix) ? messageAttribute.TopicName : $"{messageAttribute.TopicName}-{topicSuffix}";
        await _producer.ProduceAsync(topic, message);
    }
}