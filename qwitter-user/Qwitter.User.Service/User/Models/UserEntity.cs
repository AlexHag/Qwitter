using Qwitter.User.Contract.User.Models;

namespace Qwitter.User.Service.User.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public UserState UserState { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}