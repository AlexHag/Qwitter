using Qwitter.Core.Application.Kafka;

namespace Qwitter.User.Contract.Events;

[Message("user-created")]
public class UserCreatedEvent
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
}