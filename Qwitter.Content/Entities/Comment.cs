namespace Qwitter.Content.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public required string Username { get; set; }
    public required string Content { get; set; }
    public bool IsPremium { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
