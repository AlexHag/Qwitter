using MassTransit;
using Microsoft.EntityFrameworkCore;
using Qwitter.Domain.Events;
using Qwitter.Market.Database;
using Qwitter.Market.Entities;

namespace Qwitter.Market.Consumers;

public class StockSellOrderConsumer : IConsumer<StockSellOrderEvent>
{
    private readonly ILogger<StockSellOrderConsumer> _logger;
    private readonly AppDbContext _dbContext;

    public StockSellOrderConsumer(
        ILogger<StockSellOrderConsumer> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<StockSellOrderEvent> context)
    {
        var stock = await _dbContext.Stocks.Where(p => p.Ticker == context.Message.ticker).FirstOrDefaultAsync();
        if (stock is null)
        {
            _logger.LogWarning("Sell order for stock that doesn't exist... Aborting...");
            return;
        }

        var userPosition = await _dbContext.StockPositions.Where(p => 
            p.UserId == context.Message.userId &&
            p.StockId == stock.Id).FirstOrDefaultAsync();
        
        if (userPosition is null || userPosition.Quantity < context.Message.quantity)
        {
            _logger.LogWarning("User doen't own enought or any stocks to sell...");
            return;
        }

        var sellOrder = new StockSellOrder
        {
            Id = Guid.NewGuid(),
            StockId = stock.Id,
            UserId = context.Message.userId,
            Quantity = context.Message.quantity,
            Status = StockOrderStatus.Pending,
            CreatedAt = DateTime.UtcNow, // TODO: Producer should set this?
            FulfilledAt = null,
            SellPrice = context.Message.sellPrice
        };
        await _dbContext.StockSellOrders.AddAsync(sellOrder);
        await _dbContext.SaveChangesAsync();

        var buyOrders = await _dbContext.StockBuyOrders.Where(p => 
            p.StockId == stock.Id &&
            p.OrderValue() > sellOrder.SellPrice ).OrderBy(p => p.Quantity).ToListAsync();
        
        

        // var quantityMet
        foreach(var buyOrder in buyOrders)
        {

        }


        throw new NotImplementedException();
    }
}