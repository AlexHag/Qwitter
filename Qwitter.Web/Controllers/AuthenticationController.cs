using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Models.DTO;
using Qwitter.Web.Api;
using Qwitter.Web.Services;


namespace Qwitter.Web.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserClient _userClient;
    private readonly AuthenticationService _authService;

    public AuthenticationController(
        IUserClient userClient,
        AuthenticationService authService)
    {
        _userClient = userClient;
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UsernamePasswordDTO request)
    {
        try
        {
            var user = await _userClient.Login(request);
            var jwt = _authService.CreateJwt(user.Id);
            return Ok(new { user, jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UsernamePasswordDTO request)
    {
        try
        {
            var user = await _userClient.Register(request);
            var jwt = _authService.CreateJwt(user.Id);
            return Ok(new { user, jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        try
        {
            var userId = GetUserIdFromContext(HttpContext);
            var user = await _userClient.GetUser(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private Guid GetUserIdFromContext(HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        IEnumerable<Claim> claims = identity!.Claims; 
        var claimId = identity.FindFirst("Id")?.Value;
        return Guid.Parse(claimId!);
    }
}
