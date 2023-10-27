namespace Qwitter.Domain.DTO;

public class UpdateUsernameDTO
{
    public Guid UserId { get; set; }
    public required string NewUsername { get; set; }
}
