using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.User.Models;

public class UserUpdateModel
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool? HasPremium { get; set; }
    public int? FollowerCount { get; set; }
    public int? FollowingCount { get; set; }
    public UserState? UserState { get; set; }
}