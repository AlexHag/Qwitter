namespace Qwitter.Models.DTO;

public class CreatePostDTO
{
    public Guid UserId { get; set; }
    public required string Content { get; set; }
}