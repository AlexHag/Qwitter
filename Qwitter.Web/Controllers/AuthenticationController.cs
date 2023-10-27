using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Domain.DTO;
using Qwitter.Domain.Api;
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
}
