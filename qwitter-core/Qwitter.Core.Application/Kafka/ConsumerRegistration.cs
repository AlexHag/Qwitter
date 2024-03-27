namespace Qwitter.Core.Application.Kafka;

public class ConsumerRegistration
{
    public required Type ConsumerType { get; set; }
    public required Type EventType { get; set; }
    public required string TopicName { get; set; }
    public required string GroupName { get; set; }
}