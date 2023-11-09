namespace Qwitter.Market.Entities;

public class Stock
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Ticker { get; set; }
    public required string Description { get; set; }
    public double Price { get; set; }
}
