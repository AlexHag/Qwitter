namespace Qwitter.Core.Application.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationAttribute : Attribute
{
    public string Path { get; }

    public ConfigurationAttribute(string path)
    {
        Path = path;
    }
}
