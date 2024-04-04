using Qwitter.Core.Application.Kafka;

namespace Qwitter.Users.Contract.Follows.Events;

[Message("user-stopped-following")]
public class UserStoppedFollowingEvent
{
    public Guid FolloweeId { get; set; }
    public Guid FollowerId { get; set; }
}