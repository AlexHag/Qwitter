using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Qwitter.Social.Consumers;
using Qwitter.Domain.DTO;
using Qwitter.Domain.Events;
using System.Reflection;

namespace Qwitter.Social.Kafka;

public static class KafkaConfiguration
{
    public static IServiceCollection AddKafka(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.ConfigureOptions<KafkaOptionsSetup>();

        services.AddMassTransit(bus =>
        {
            bus.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

            bus.AddRider(rider =>
            {
                var options = services.BuildServiceProvider().GetRequiredService<IOptions<KafkaOptions>>().Value;
                
                rider.AddConsumers(assemblies);

                rider.AddProducer<UpdateUsernameDTO>(options.UsernameChangedTopicName);
                rider.UsingKafka((context, configurator) =>
                {
                    configurator.ConfigureHost(options);
                    configurator.ClientId = "Qwitter";

                    configurator.AddTopicEndpoints(options, context);
                });
            });
        });

        return services;
    }

    private static void AddTopicEndpoints(this IKafkaFactoryConfigurator configurator, KafkaOptions options, IRiderRegistrationContext context)
    {
        configurator.TopicEndpoint<UpdateUsernameDTO>(options.UsernameChangedTopicName, options.SocialConsumerGroupId,
            endpointConfigurator => endpointConfigurator.ConfigureEndpointSettings<UsernameChangedConsumer>(context));
        
        configurator.TopicEndpoint<PremiumPurchasedSuccessfullyEvent>(options.PremiumPurchasedSuccessfullyTopicName, options.SocialConsumerGroupId,
            endpointConfigurator => endpointConfigurator.ConfigureEndpointSettings<PremiumPurchasedConsumer>(context));
    }

    private static void ConfigureEndpointSettings<TConsumer>(
        this IKafkaTopicReceiveEndpointConfigurator configurator, IRiderRegistrationContext context)
        where TConsumer : class, IConsumer
    {
        configurator.AutoOffsetReset = AutoOffsetReset.Earliest;
        configurator.ConcurrentMessageLimit = 10;
        configurator.UseKillSwitch(k =>
            k.SetActivationThreshold(1).SetRestartTimeout(m: 1).SetTripThreshold(0.2).SetTrackingPeriod(m: 1));
        configurator.UseMessageRetry(retry => retry.Interval(1000, TimeSpan.FromSeconds(1)));
        configurator.ConfigureConsumer<TConsumer>(context);
    }

    private static void ConfigureHost(this IKafkaFactoryConfigurator configurator, KafkaOptions options)
    {
        configurator.Host(options.BootstrapServers, h =>
        {
            if (!string.IsNullOrWhiteSpace(options.Username) &&
                !string.IsNullOrWhiteSpace(options.Password))
            {
                h.UseSasl(s =>
                {
                    s.SecurityProtocol = SecurityProtocol.SaslSsl;
                    s.Mechanism = SaslMechanism.ScramSha256;
                    s.Username = options.Username;
                    s.Password = options.Password;
                });
            }
        });
    }
}

