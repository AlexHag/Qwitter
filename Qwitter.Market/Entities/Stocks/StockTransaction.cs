namespace Qwitter.Market.Entities;

public class StockBuyTransaction
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Guid UserId { get; set; }
    public double BoughtAt { get; set; }
    public int Quantity { get; set; }
    public double OrderTotal => BoughtAt * Quantity;
    public DateTime CreatedAt { get; set; }
}

public class StockSellTransaction
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Guid UserId { get; set; }
    public double SoldAt { get; set; }
    public int Quantity { get; set; }
    public double OrderTotal => SoldAt * Quantity;
    public DateTime CreatedAt { get; set; }
}

public class StockTransaction
{
    public Guid Id { get; set; }
    public Guid BuyOrderId { get; set; }
    public Guid SellOrderId { get; set; }
    public Guid UserId { get; set; }
    public double SoldAt { get; set; }
    public int Quantity { get; set; }
    public double OrderTotal => SoldAt * Quantity;
    public DateTime CreatedAt { get; set; }
}