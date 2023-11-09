namespace Qwitter.Market.Entities;

public class StockPosition
{
    public Guid Id { get; set; }
    public Guid StockId { get; set; }
    public Guid UserId { get; set; }
    public int Quantity { get; set; }
}
