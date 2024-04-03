namespace Qwitter.Core.Application.Kafka;

[AttributeUsage(AttributeTargets.Class)]
public class MessageSuffixAttribute(string suffix) : Attribute
{
    public string Suffix { get; } = suffix;
}