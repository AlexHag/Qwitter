namespace Qwitter.Users.Follows.Models;

public class FollowingRelationshipEntity
{
    public Guid Id { get; set; }
    public Guid FolloweeId { get; set; }
    public Guid FollowerId { get; set; }
    public DateTime CreatedAt { get; set; }
}