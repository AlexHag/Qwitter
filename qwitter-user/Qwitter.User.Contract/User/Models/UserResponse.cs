namespace Qwitter.User.Contract.User.Models;

public class UserResponse
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public UserState UserState { get; set; }
}
