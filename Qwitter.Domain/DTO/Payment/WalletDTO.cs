namespace Qwitter.Domain.DTO;

public class UserWalletDTO
{
    public required string Address { get; set; }
    public decimal Balance { get; set; }
}