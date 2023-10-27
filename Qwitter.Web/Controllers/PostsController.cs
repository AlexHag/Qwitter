using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Domain.Api;
using Qwitter.Domain.DTO;
using Qwitter.Web.Services;

namespace Qwitter.Web.Controllers;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IContentClient _contentClient;

    public PostsController(IContentClient content)
    {
        _contentClient = content;
    }

    [HttpPost]
    [Route("create")]
    [Authorize]
    public async Task<IActionResult> CreatePost([FromBody] string content)
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            var creatPostDTO = new CreatePostDTO
            {
                UserId = userId,
                Content = content
            };
            var post = await _contentClient.CreatePost(creatPostDTO);
            return Ok(post);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("{username}")]
    public async Task<IActionResult> GetUserPosts(string username)
    {
        try
        {
            var posts = await _contentClient.GetUserPosts(username);
            return Ok(posts);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("a/{postId}")]
    public async Task<IActionResult> GetPostById(Guid postId) 
    {
        try
        {
            var post = await _contentClient.GetPostById(postId);
            return Ok(post);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{postId}/like")]
    public async Task<IActionResult> LikePost(Guid postId)
    {
        try
        {
            await _contentClient.LikePost(postId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{postId}/dislike")]
    public async Task<IActionResult> DislikePost(Guid postId)
    {
        try
        {
            await _contentClient.DislikePost(postId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
