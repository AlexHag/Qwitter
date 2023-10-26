namespace Qwitter.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Salt { get; set; }
    public bool IsPremium { get; set; }
}
