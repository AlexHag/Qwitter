namespace Qwitter.Payments.Entities;

public abstract class Purchasable
{
    public Purchasable(PurchasableItemCategories category)
    {
        ItemCategory = category;
    }

    public Guid Id { get; set; }
    public double Price { get; set; }
    public required string PaymentAddress { get; set; }
    public PurchasableItemCategories ItemCategory { get; private init; }
}

public enum PurchasableItemCategories
{
    Unknown = 0,
    Subscription = 1,
    Instrument = 2,
}


public class Premium : Purchasable
{
    public Premium(): base(PurchasableItemCategories.Subscription)
    { }
}


public class Instrument : Purchasable
{
    public Instrument() : base(PurchasableItemCategories.Instrument)
    { }
    public required string Name { get; set; }
    public required string ISIN { get; set; }
    public InstrumentCategories InstrumentCategory { get; set; }
}

public enum InstrumentCategories
{
    Unknown = 0,
    Stock = 1,
    Option = 2,
    Future = 3,
    Bonds = 4
}
