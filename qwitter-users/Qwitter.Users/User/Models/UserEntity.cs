using Qwitter.Core.Application.Persistence;

namespace Qwitter.Users.User.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool HasPremium { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public UserState UserState { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}