
namespace Qwitter.Core.Application.RestApiClient;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiHostAttribute(string port) : Attribute
{
    public string Port { get; } = port;
}