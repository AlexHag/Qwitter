namespace Qwitter.Users.User.Models;

public class UserInsertModel
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
