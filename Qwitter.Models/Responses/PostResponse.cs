namespace Qwitter.Models.Responses;

public class PostResponse
{
    public Guid PostId { get; set; }
    public Guid AuthorUserId { get; set; }
    public required string AuthorUsername { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool IsPremium { get; set; }
}