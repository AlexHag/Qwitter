using Qwitter.Core.Application.Kafka;

namespace Qwitter.Users.Contract.Follows.Events;

[Message("user-started-following")]
public class UserStartedFollowingEvent
{
    public Guid FolloweeId { get; set; }
    public Guid FollowerId { get; set; }
}