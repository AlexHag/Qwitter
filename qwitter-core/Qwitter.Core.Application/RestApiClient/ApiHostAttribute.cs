
namespace Qwitter.Core.Application.RestApiClient;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiHostAttribute(string port, string prefix = "") : Attribute
{
    public string Port { get; } = port;
    public string Prefix { get; } = prefix;
}