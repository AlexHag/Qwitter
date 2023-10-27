using Microsoft.AspNetCore.Mvc;
using Qwitter.Content.Database;
using Qwitter.Content.Entities;
using Qwitter.Domain.DTO;
using Qwitter.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace Qwitter.Content.Controllers;

[ApiController]
[Route("comments")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IUserClient _userClient;

    public CommentsController(
        AppDbContext dbContext,
        IUserClient userClient)
    {
        _dbContext = dbContext;
        _userClient = userClient;
    }

    [HttpPost("{postId}")]
    public async Task<IActionResult> CreateComment(CreateCommentDTO request)
    {
        try
        {
            var user = await _userClient.GetUser(request.UserId);
            var post = await _dbContext.Posts.FindAsync(request.PostId);
            if (post is null)
            {
                return NotFound("Post not found");
            }

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                PostId = post.Id,
                Username = user.Username,
                Content = request.Content,
                IsPremium = user.IsPremium,
                Likes = 0,
                Dislikes = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            post.Comments.Add(comment);
            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Username = comment.Username,
                Content = comment.Content,
                IsPremium = comment.IsPremium,
                Likes = comment.Likes,
                Dislikes = comment.Dislikes,
                CreatedAt = comment.CreatedAt,
            };
            return Ok(commentDTO);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}