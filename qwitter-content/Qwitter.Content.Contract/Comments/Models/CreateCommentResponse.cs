namespace Qwitter.Content.Contract.Comments.Models;

public class CreateCommentResponse
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public required string Content { get; set; }
}