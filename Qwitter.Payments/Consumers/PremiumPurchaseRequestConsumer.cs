using MassTransit;
using Qwitter.Domain.Events;
using Qwitter.Payments.Database;
using Qwitter.Payments.Service;
using Qwitter.Payments.Entities;

namespace Qwitter.Payments.Consumers;

public class PremiumPurchaseRequestConsumer : IConsumer<PremiumPurchaseRequestedEvent>
{
    private readonly ILogger<PremiumPurchaseRequestConsumer> _logger;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;
    private readonly INethereumService _nethereumService;
    private readonly ITopicProducer<PremiumPurchasedSuccessfullyEvent> _premiumPurchasedSuccessfullyEventProducer;

    public PremiumPurchaseRequestConsumer(
        ILogger<PremiumPurchaseRequestConsumer> logger,
        IConfiguration configuration,
        AppDbContext dbContext,
        INethereumService nethereumService,
        ITopicProducer<PremiumPurchasedSuccessfullyEvent> premiumPurchasedSuccessfullyEventProducer)
    {
        _logger = logger;
        _configuration = configuration;
        _dbContext = dbContext;
        _nethereumService = nethereumService;
        _premiumPurchasedSuccessfullyEventProducer = premiumPurchasedSuccessfullyEventProducer;
    }

    public async Task Consume(ConsumeContext<PremiumPurchaseRequestedEvent> context)
    {
        _logger.LogInformation("Consuming premium purchase requested event");
        var wallet = await _dbContext.UserWallets.FindAsync(context.Message.walletId);
        if (wallet is null)
        {
            _logger.LogWarning($"Could not find walletId {context.Message.walletId}... Aborting");
            return;
        }

        var toAddress = _configuration["Premium:QwitterPremiumWalletAddress"]!;
        var premiumPrice = Decimal.Parse(_configuration["Premium:QwitterPremiumPrice"]!);

        var transactionTask = _nethereumService.SendTransaction
        (
            wallet.PrivateMnemonic,
            toAddress,
            premiumPrice
        );

        var qwitterTransaction = new QwitterTransaction
        {
            Id = Guid.NewGuid(),
            FromAddress = wallet.Address,
            ToAddress = toAddress,
            Amount = premiumPrice,
            Status = QwitterTransactionStatus.Pending,
            CreatedAt = context.Message.CreatedAt
        };
        await _dbContext.QwitterTransactions.AddAsync(qwitterTransaction);
        await _dbContext.SaveChangesAsync();

        try
        {
            var transaction = await transactionTask;
            _logger.LogInformation("Transaction created successfully, updating transaction details in database");
            // Simulate latency
            await Task.Delay(10000);
            qwitterTransaction.TransactionHash = transaction.TransactionHash;
            qwitterTransaction.CompletedAt = DateTime.UtcNow;
            qwitterTransaction.Status = QwitterTransactionStatus.Completed;
            await _dbContext.SaveChangesAsync();

            await _premiumPurchasedSuccessfullyEventProducer.Produce(new PremiumPurchasedSuccessfullyEvent
            (
                wallet.UserId
            ), context.CancellationToken);

            var newBalance = await _nethereumService.CheckBalance(wallet.Address);
            wallet.Balance = newBalance;
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Transaction failed, {e.Message}");
            qwitterTransaction.Status = QwitterTransactionStatus.Failed;
        }
    }
}