namespace Qwitter.Payment.Entities;

public class UserWallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string PrivateMnemonic { get; set; }
    public required string Address { get; set; }
    public decimal Balance { get; set; }
}
