using System.ComponentModel.DataAnnotations;

namespace Qwitter.Models.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Salt { get; set; }
    public bool IsPremium { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
}