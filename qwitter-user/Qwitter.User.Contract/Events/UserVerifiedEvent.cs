using Qwitter.Core.Application.Kafka;

namespace Qwitter.User.Contract.Events;

[Message("user-verified")]
public class UserVerifiedEvent
{
    public Guid UserId { get; set; }
}
