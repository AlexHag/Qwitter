using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class Posts
{
    [Key]
    public Guid PostId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }

    public Guid UserId { get; set; }
    public Guid PublicUserId { get; set; }
    public long TimePosted { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool IsPremium { get; set; }
}

public class PostsAndUser
{
    public Guid PostId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }

    public Guid UserId { get; set; }
    public Guid PublicUserId { get; set; }
    public long TimePosted { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool IsPremium { get; set; }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Guid PublicId { get; set; }
}

public class ClientPostModel
{
    public Guid UserId { get; set; }
    public string Content { get; set; }
}

public class ClientSendPostModel
{
    public Guid PostId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public Guid PublicUserId { get; set; }
    public long TimePosted { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public bool IsPremium { get; set; }

}