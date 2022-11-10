using System.ComponentModel.DataAnnotations;

namespace server.Models;

public class Comment
{
    [Key]
    public Guid CommentId { get; set; }
    public Guid RelatedPostId { get; set; }
    public Guid UserId { get; set; }
    public Guid PublicUserId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public long TimePosted { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}

public class ClientMakeCommentModel
{
    public Guid RelatedPostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
}

// public class ClientGetCommentModel
// {
//     public Guid RelatedPostId { get; set; }
//     public Guid UserId { get; set; }
// }

public class ClientSendCommentModel
{
    public Guid CommentId { get; set; }
    public Guid RelatedPostId { get; set; }
    public Guid PublicUserId { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public long TimePosted { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}