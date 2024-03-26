using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.Auth.Models;

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}