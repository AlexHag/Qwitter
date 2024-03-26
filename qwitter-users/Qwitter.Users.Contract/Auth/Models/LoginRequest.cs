using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.Auth.Models;

public class LoginRequest
{
    public required string UsernameOrEmail { get; set; }

    public required string Password { get; set; }
}