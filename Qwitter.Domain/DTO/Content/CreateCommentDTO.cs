namespace Qwitter.Domain.DTO;

public class CreateCommentDTO
{
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public required string Content { get; set; }
}