namespace Qwitter.Domain.DTO;

public class UserDTO
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public string? Bio { get; set; }
    public bool IsPremium { get; set; }
}