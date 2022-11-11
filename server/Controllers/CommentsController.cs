using Microsoft.AspNetCore.Mvc;
using server.Context;
using server.Models;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    public dataContext _context;
    public CommentsController(dataContext context)
    {
        _context = context;
    }

    [Route("/GetCommentsOnPost/{id}")]
    [HttpGet]
    public ActionResult<IEnumerable<ClientSendCommentModel>> GetCommentsOnPost(Guid id)
    {
        var comments = from c in _context.Comments where c.RelatedPostId == id select c;
        if(comments.FirstOrDefault() is null) return NotFound();
        
        var commentsToSend = from c in comments select new ClientSendCommentModel{
            CommentId = c.CommentId,
            RelatedPostId = c.RelatedPostId,
            PublicUserId = c.PublicUserId,
            Author = c.Author,
            Content = c.Content,
            TimePosted = c.TimePosted,
            Likes = c.Likes,
            Dislikes = c.Dislikes,
            isPremium = c.isPremium
        };

        return commentsToSend.OrderByDescending(p => p.TimePosted).ToList();
    }

    [Route("/CommentOnPost")]
    [HttpPost]
    public ActionResult CommentOnPost(ClientMakeCommentModel commentInput)
    {
        var author = from u in _context.Users where u.Id == commentInput.UserId select u.Username;
        if(author.FirstOrDefault() is null) return BadRequest("Unknown userID");
        var pubId = from u in _context.Users where u.Id == commentInput.UserId select u.PublicId;
        var isPremium = from u in _context.Users where u.Id == commentInput.UserId select u.IsPremium;
        //if(pubId.FirstOrDefault() is null) return BadRequest("Unknown userID");

        if(_context.Posts.Find(commentInput.RelatedPostId) is null) return NotFound("Post don't exist");

        var newComment = new Comment{
            CommentId = Guid.NewGuid(),
            RelatedPostId = commentInput.RelatedPostId,
            UserId = commentInput.UserId,
            PublicUserId = pubId.FirstOrDefault(),
            Author = author.FirstOrDefault(),
            Content = commentInput.Content,
            TimePosted = DateTimeOffset.Now.ToUnixTimeSeconds(),
            Likes = 0,
            Dislikes = 0,
            isPremium = isPremium.FirstOrDefault()
        };

        _context.Comments.Add(newComment);
        _context.SaveChanges();
        return StatusCode(201);
    }

}