namespace Qwitter.Domain.Events;

public record StockSellOrderEvent
(
    Guid userId,
    string ticker, // TODO: does the producer know the StockId? Use name or ticker?
    double sellPrice,
    int quantity
);
