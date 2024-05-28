using MassTransit;
using Qwitter.Crypto.Contract.Wallets.Events;

namespace Qwitter.Crypto.SystemLedger.Consumers;

public class CryptoDepositConsumer : IConsumer<CryptoDepositEvent>
{
    private readonly ILogger<CryptoDepositConsumer> _logger;

    public CryptoDepositConsumer(ILogger<CryptoDepositConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CryptoDepositEvent> context)
    {
        _logger.LogInformation("Received CryptoDepositEvent: {Event}", context.Message);
        return Task.CompletedTask;
    }
}