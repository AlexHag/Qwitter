namespace Qwitter.Domain.DTO;

public class PostDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Content { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<CommentDTO>? Comments { get; set; }
    public bool IsPremium { get; set; }
    public bool Edited { get; set; }
    public DateTime CreatedAt { get; set; }
}