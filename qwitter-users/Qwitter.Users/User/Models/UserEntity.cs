using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.User.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool HasPremium { get; set; }
    // public bool IsPrivate { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    // public int PostCount { get; set; }
    // public string? Bio { get; set; }
    // public string? ProfilePictureUrl { get; set; }
    public UserState UserState { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}