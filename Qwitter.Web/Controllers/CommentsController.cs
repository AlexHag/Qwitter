using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Domain.Api;
using Qwitter.Domain.DTO;
using Qwitter.Web.Services;

namespace Qwitter.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IContentClient _contentClient;

    public CommentsController(IContentClient content)
    {
        _contentClient = content;
    }

    [HttpPost]
    [Route("create/{postId}")]
    [Authorize]
    public async Task<IActionResult> CreateComment(Guid postId, [FromBody] string content)
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            var createCommentDTO = new CreateCommentDTO
            {
                UserId = userId,
                PostId = postId,
                Content = content
            };
            var comment = await _contentClient.CreateComment(createCommentDTO);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
