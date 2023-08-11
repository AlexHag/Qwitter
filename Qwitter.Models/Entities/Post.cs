using System.ComponentModel.DataAnnotations;

namespace Qwitter.Models.Entities;

public class Post
{
    [Key]
    public Guid PostId { get; set; }
    public required User Author { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool IsPremium { get; set; }
}