namespace Qwitter.Core.Application.Kafka;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute(string topicName) : Attribute
{
    public string TopicName { get; } = topicName;
}