using Qwitter.Core.Application.Persistence;

namespace Qwitter.Payments.User.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public UserState UserState { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}
