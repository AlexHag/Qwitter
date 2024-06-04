namespace Qwitter.Content.Contract.Comments.Models;

public class CreateCommentRequest
{
    public Guid PostId { get; set; }
    public required string Content { get; set; }
}