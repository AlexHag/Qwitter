using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.Auth.Models;

public class AuthResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}