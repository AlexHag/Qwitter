namespace Qwitter.Users.Requests;

public class UsernamePasswordRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
