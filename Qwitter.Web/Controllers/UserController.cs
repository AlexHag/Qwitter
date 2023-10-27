using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Domain.Api;
using Qwitter.Domain.DTO;
using Qwitter.Web.Services;

namespace Qwitter.Web.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserClient _userClient;

    public UserController(IUserClient userClient)
    {
        _userClient = userClient;
    }

    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            var user = await _userClient.GetUser(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("bio")]
    [Authorize]
    public async Task<IActionResult> UpdateBio([FromBody] string bio)
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            await _userClient.UpdateBio(new UpdateBioDTO
            {
                UserId = userId,
                Bio = bio
            });
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    [Route("username")]
    [Authorize]
    public async Task<IActionResult> UpdateUsername([FromBody] string newUsername)
    {
        try
        {
            var userId = AuthenticationService.GetUserIdFromContext(HttpContext);
            await _userClient.UpdateUsername(new UpdateUsernameDTO
            {
                UserId = userId,
                NewUsername = newUsername
            });
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
