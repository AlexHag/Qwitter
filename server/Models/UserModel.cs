using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid PublicId { get; set; }
    public string? PrivateMnemonic { get; set; }
    public string? PublicAddress { get; set; }
    public bool IsPremium { get; set; }
}

public class ClientUserModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}