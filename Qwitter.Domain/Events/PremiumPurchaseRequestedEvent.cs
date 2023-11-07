namespace Qwitter.Domain.Events;

// TODO: Add sell all flag, or Amount + Correct Fees
public record PremiumPurchaseRequestedEvent
(
    Guid walletId,
    DateTime CreatedAt
);
