using System.Text.Json.Serialization;

namespace Qwitter.Users.Contract.User.Models;

public class UserProfile
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserState UserState { get; set; }
}