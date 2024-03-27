using System.Reflection;
using Confluent.Kafka;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Qwitter.Core.Application.Kafka;

public static class KafkaExtensions
{
    private static List<ConsumerRegistration> _consumers = new();

    public static WebApplicationBuilder RegisterConsumer<TConsumer>(this WebApplicationBuilder builder, string groupName)
        where TConsumer : class, IConsumer
    {
        var consumerInterface = typeof(TConsumer)
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>));

        if (consumerInterface is null)
        {
            throw new InvalidOperationException($"Consumer {typeof(TConsumer).Name} does not implement IConsumer<T>");
        }

        var eventType = consumerInterface.GetGenericArguments().FirstOrDefault();

        var messageAttribute = eventType?.GetCustomAttribute<MessageAttribute>();

        if (messageAttribute is null)
        {
            throw new InvalidOperationException($"Event {eventType} does not have a MessageAttribute");
        }

        _consumers.Add(new ConsumerRegistration
        {
            ConsumerType = typeof(TConsumer),
            EventType = eventType!,
            TopicName = messageAttribute.TopicName,
            GroupName = groupName
        });

        return builder;
    }

    public static WebApplicationBuilder UseKafka(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(bus =>
        {
            bus.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

            bus.AddRider(rider =>
            {
                foreach (var consumer in _consumers)
                {
                    rider.AddConsumer(consumer.ConsumerType);
                }

                rider.UsingKafka((context, configurator) =>
                {
                    configurator.Host(builder.Configuration["Kafka:BootstrapServers"], h =>
                    {
                        if (!string.IsNullOrWhiteSpace(builder.Configuration["Kafka:Username"]) &&
                            !string.IsNullOrWhiteSpace(builder.Configuration["Kafka:Password"]))
                        {
                            h.UseSasl(s =>
                            {
                                s.SecurityProtocol = SecurityProtocol.SaslSsl;
                                s.Mechanism = SaslMechanism.ScramSha256;
                                s.Username = builder.Configuration["Kafka:Username"];
                                s.Password = builder.Configuration["Kafka:Password"];
                            });
                        }
                    });

                    foreach (var consumer in _consumers)
                    {
                        var method = typeof(KafkaExtensions)
                            .GetMethod(nameof(AddConsumerToTopic), BindingFlags.Static | BindingFlags.NonPublic);

                        if (method is null)
                        {
                            throw new InvalidOperationException("Method 'AddConsumerToTopic' not found.");
                        }

                        var genericMethod = method.MakeGenericMethod(consumer.ConsumerType, consumer.EventType);

                        genericMethod.Invoke(null, [configurator, context, consumer.TopicName, consumer.GroupName]);
                    }
                });
            });
        });

        builder.ConfigureProducer();

        return builder;
    }

    private static WebApplicationBuilder ConfigureProducer(this WebApplicationBuilder builder)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = builder.Configuration["Kafka:BootstrapServers"]
        };

        if (!string.IsNullOrWhiteSpace(builder.Configuration["Kafka:Username"]) &&
            !string.IsNullOrWhiteSpace(builder.Configuration["Kafka:Password"]))
        {
            producerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
            producerConfig.SaslMechanism = SaslMechanism.ScramSha256;
            producerConfig.SaslUsername = builder.Configuration["Kafka:Username"];
            producerConfig.SaslPassword = builder.Configuration["Kafka:Password"];   
        }

        var producer = new ProducerBuilder<string, string>(producerConfig).Build();

        builder.Services.AddScoped<IEventProducer>(p => new EventProducer(producer));

        return builder;
    }

    private static void AddConsumerToTopic<TConsumer, TEvent>(IKafkaFactoryConfigurator configurator, IRiderRegistrationContext context, string topic, string group)
        where TConsumer : class, IConsumer<TEvent>
        where TEvent : class
    {
        configurator.TopicEndpoint<TEvent>(topic, group, e => e.ConfigureEndpointSettings<TConsumer>(context));
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

}