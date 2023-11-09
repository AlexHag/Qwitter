namespace Qwitter.Products.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double Price { get; set; }
    public required string PaymentAddress { get; set; }
}
