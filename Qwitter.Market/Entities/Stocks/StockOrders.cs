namespace Qwitter.Market.Entities;

public abstract class StockOrder
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Guid UserId { get; set; }
    public int Quantity { get; set; }
    public abstract double OrderValue();
    public StockOrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
}

public class StockBuyOrder : StockOrder
{
    public double BuyPrice { get; set; }
    public override double OrderValue() => BuyPrice * Quantity;
}

public class StockSellOrder : StockOrder
{
    public double SellPrice { get; set; }
    public override double OrderValue() => SellPrice * Quantity;
}

public enum StockOrderStatus
{
    Unknown = 1,
    Pending = 2,
    Fulfilled = 3,
    PartiallyFulfilled = 4,
    Cancelled = 5
}
