namespace Qwitter.Content.Users.Models;

public class UserInsertModel
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
}