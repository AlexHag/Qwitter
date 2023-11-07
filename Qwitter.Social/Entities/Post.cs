namespace Qwitter.Social.Entities;

public class Post
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Content { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
    public bool IsPremium { get; set; }
    public bool Edited { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
}
