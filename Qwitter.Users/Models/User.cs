namespace Qwitter.Users.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public string? Bio { get; set; }
    public required string PasswordHash { get; set; }
    public required string Salt { get; set; }
    public bool IsPremium { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
