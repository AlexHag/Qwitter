using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Service.Database.Repositories;
using Qwitter.Models.Requests;
using System.Security.Claims;

[ApiController]
[Route("api")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    [Route("UploadPost")]
    [Authorize]
    public async Task<IActionResult> UploadPost(UploadPostRequest request)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = Guid.Parse(identity!.FindFirst("Id")!.Value);
        var newPost = await _postRepository.UploadPost(userId, request.Content);
        
        return Ok(newPost);
    }

    [HttpGet]
    [Route("Post/{id}")]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var post = await _postRepository.GetPostById(postId);
        if (post is null) return NotFound();

        return Ok(post);
    }

    [HttpGet]
    [Route("UsersPosts")]
    public async Task<IActionResult> GetUsersPosts()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = Guid.Parse(identity!.FindFirst("Id")!.Value);

        var usersPosts = await _postRepository.GetUsersPosts(userId);
        return Ok(usersPosts);
    }
}