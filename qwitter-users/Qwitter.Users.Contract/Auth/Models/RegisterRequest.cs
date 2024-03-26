using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.Auth.Models;

public class RegisterRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("password")]
    public required string Password { get; set; }
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}