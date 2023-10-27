using Microsoft.AspNetCore.Mvc;
using Qwitter.Content.Database;
using Qwitter.Content.Entities;
using Qwitter.Domain.DTO;
using Qwitter.Domain.Api;
using Microsoft.EntityFrameworkCore;

namespace Qwitter.Content.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IUserClient _userClient;

    public PostsController(
        ILogger<PostsController> logger,
        AppDbContext dbContext,
        IUserClient userClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userClient = userClient;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePost(CreatePostDTO request)
    {
        try
        {
            var user = await _userClient.GetUser(request.UserId);

            var post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Username = user.Username,
                Content = request.Content,
                Likes = 0,
                Dislikes = 0,
                IsPremium = user.IsPremium,
                Edited = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
            
            var postDTO = new PostDTO
            {
                Id = post.Id,
                UserId = post.Id,
                Username = post.Username,
                Content = post.Content,
                Likes = 0,
                Dislikes = 0,
                IsPremium = post.IsPremium,
                Edited = false,
                CreatedAt = post.CreatedAt
            };

            return Ok(postDTO);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var post = await _dbContext.Posts.Include(p => p.Comments).Where(p => p.Id == postId).FirstOrDefaultAsync();
        if (post is null)
        {
            return NotFound("Post not found");
        }

        var postDTO = new PostDTO
        {
            Id = post.Id,
            UserId = post.Id,
            Username = post.Username,
            Content = post.Content,
            Likes = post.Likes,
            Dislikes = post.Dislikes,
            Comments = post.Comments.Select(comment => new CommentDTO
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
            }).ToList(),
            IsPremium = post.IsPremium,
            Edited = false,
            CreatedAt = post.CreatedAt
        };

        return Ok(postDTO);
    }

    [HttpGet("user/{username}")]
    public async Task<IActionResult> GetUserPosts(string username)
    {
        var posts = await _dbContext.Posts.Where(p => p.Username == username).ToListAsync();
        if (posts is null)
        {
            return NotFound("User has no posts");
        }

        var postsDTO = posts.Select(p => new PostDTO
        {
            Id = p.Id,
            UserId = p.Id,
            Username = p.Username,
            Content = p.Content,
            Likes = p.Likes,
            Dislikes = p.Dislikes,
            Comments = null,
            IsPremium = p.IsPremium,
            Edited = false,
            CreatedAt = p.CreatedAt
        });
        return Ok(postsDTO);
    }

    [HttpPost("{postId}/like")]
    public async Task<IActionResult> LikePost(Guid postId)
    {
        var post = await _dbContext.Posts.FindAsync(postId);
        if (post is null)
        {
            return NotFound("Post not found");
        }

        post.Likes += 1;
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("{postId}/dislike")]
    public async Task<IActionResult> DislikePost(Guid postId)
        {
        var post = await _dbContext.Posts.FindAsync(postId);
        if (post is null)
        {
            return NotFound("Post not found");
        }

        post.Dislikes += 1;
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}
