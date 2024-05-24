using System.Text.Json.Serialization;
using Qwitter.Core.Application.Persistence;

namespace Qwitter.Users.Contract.User.Models;

public class UserProfile
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public bool HasPremium { get; set; }
    // public bool IsPrivate { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    // public int PostCount { get; set; }
    // public string? Bio { get; set; }
    // public string? ProfilePictureUrl { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserState UserState { get; set; }
}