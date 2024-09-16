namespace Qwitter.User.Contract.Auth.Models;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}