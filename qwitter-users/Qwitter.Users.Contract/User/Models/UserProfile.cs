namespace Qwitter.Users.Contract.User.Models;

public class UserProfile
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public UserState State { get; set; }
}