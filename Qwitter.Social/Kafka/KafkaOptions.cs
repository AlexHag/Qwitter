using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Qwitter.Social.Kafka;

public class KafkaOptions
{
    public string UsernameChangedTopicName { get; set; } = string.Empty;
    public string PremiumPurchasedSuccessfullyTopicName { get; set; } = string.Empty;
    public string SocialConsumerGroupId { get; set; } = string.Empty;

    public string BootstrapServers { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class KafkaOptionsSetup : IConfigureOptions<KafkaOptions>
{
    private const string SectionName = "Kafka";
    private readonly IConfiguration _configuration;

    public KafkaOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(KafkaOptions options)
    {
        _configuration
            .GetSection(SectionName)
            .Bind(options);
    }
}
