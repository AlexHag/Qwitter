using Qwitter.Core.Application.Kafka;
using Qwitter.Core.Application.Persistence;

namespace Qwitter.Users.Contract.User.Events;

[Message("user-state-changed")]
public class UserStateChangedEvent
{
    public Guid UserId { get; set; }
    public UserState UserState { get; set; }
}