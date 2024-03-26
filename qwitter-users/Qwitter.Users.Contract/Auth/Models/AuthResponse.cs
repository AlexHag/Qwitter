using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.Auth.Models;

public class AuthResponse
{
    public required string Token { get; set; }
}