namespace Qwitter.Domain.DTO;

public class UpdateBioDTO
{
    public Guid UserId { get; set; }
    public required string Bio { get; set; }
}