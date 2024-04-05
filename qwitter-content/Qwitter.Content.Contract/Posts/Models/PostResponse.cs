namespace Qwitter.Content.Contract.Posts.Models;

public class PostResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Content { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTime CreatedAt { get; set; }
}
