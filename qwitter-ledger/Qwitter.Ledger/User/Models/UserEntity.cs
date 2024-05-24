using Qwitter.Core.Application.Persistence;

namespace Qwitter.Ledger.User.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public Guid? DefaultAccountId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public UserState UserState { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}
