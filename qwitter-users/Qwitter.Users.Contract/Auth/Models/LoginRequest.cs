using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.Auth.Models;

public class LoginRequest
{
    [JsonPropertyName("usernameOrEmail")]
    public required string UsernameOrEmail { get; set; }

    [JsonPropertyName("password")]
    public required string Password { get; set; }
}