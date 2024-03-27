using Qwitter.Core.Application.Kafka;

namespace Qwitter.Users.Contract.User.Events;

[Message("user-created")]
public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
}