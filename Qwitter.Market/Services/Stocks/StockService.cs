using Qwitter.Market.Entities;
using Qwitter.Market.Database;
using Microsoft.EntityFrameworkCore;

namespace Qwitter.Market.Services;

public class StockService
{
    private readonly ILogger<StockService> _logger;
    private readonly AppDbContext _dbContext;

    public StockService(
        ILogger<StockService> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task HandleSellOrder(StockSellOrder sellOrder)
    {
        var buyOrders = await _dbContext.StockBuyOrders.Where(p => 
            p.StockId == sellOrder.StockId &&
            p.OrderValue() > sellOrder.SellPrice )
            .OrderByDescending(p => p.BuyPrice)
            .ToListAsync();
        
        if (buyOrders is null ||buyOrders.Count < 1)
        {
            _logger.LogDebug("No buy orders for sell order...");
            return;
        }

        var quantityFilled = 0;
        var selectedBuyOrders = new List<StockBuyOrder>();
        foreach (var buyOrder in buyOrders)
        {
            quantityFilled += buyOrder.Quantity;
            
        }
        
    }

    public async Task FulfillBuyOrder(Guid buyOrderId)
    {

    }
}