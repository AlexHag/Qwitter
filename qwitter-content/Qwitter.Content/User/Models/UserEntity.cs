namespace Qwitter.Content.Users.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public bool HasPremium { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}