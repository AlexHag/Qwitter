using Qwitter.Content.Users.Models;

namespace Qwitter.Content.Posts.Models;

public class PostEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required UserEntity User { get; set; }
    public required string Content { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
