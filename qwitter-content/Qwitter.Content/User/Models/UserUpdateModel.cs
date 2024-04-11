namespace Qwitter.Content.Users.Models;

public class UserUpdateModel
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public bool? HasPremium { get; set; }
}