using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Qwitter.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    // private readonly ILogger<UserController> _logger;
    // private readonly IUserService _service;

    // public UserController(
    //     ILogger<UserController> logger,
    //     IUserService service)
    // {
    //     _logger = logger;
    //     _service = service;
    // }

    // [HttpPost]
    // [Route("login")]
    // public async Task<IActionResult> Login(UsernamePasswordRequest request)
    // {
    //     try
    //     {
    //         var jwt = await _service.Login(request);
    //         return Ok(new { jwt });    
    //     }
    //     catch (LoginFailedException e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }

    // [HttpPost]
    // [Route("register")]
    // public async Task<IActionResult> Register(UsernamePasswordRequest request)
    // {
    //     try
    //     {
    //         var jwt = await _service.Register(request);
    //         return Ok(new { jwt });    
    //     }
    //     catch (UsernameAlreadyExistException)
    //     {
    //         return BadRequest("Username already exists");
    //     }
    // }

    // [HttpGet]
    // [Route("me")]
    // [Authorize]
    // public async Task<IActionResult> Me()
    // {
    //     try
    //     {
    //         var userId = GetUserIdFromContext(HttpContext);
    //         var user =  await _service.GetUserById(userId);
    //         return Ok(user);
    //     }
    //     catch (NotFoundException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }

    // [HttpPost]
    // [Route("premium")]
    // [Authorize]
    // public async Task<IActionResult> Premium()
    // {
    //     try
    //     {
    //         var userId = GetUserIdFromContext(HttpContext);
    //         await _service.MakeUserPremium(userId);
    //         return Ok();
    //     }
    //     catch (NotFoundException e)
    //     {
    //         return NotFound(e.Message);
    //     }
    // }

    // private Guid GetUserIdFromContext(HttpContext context)
    // {
    //     var identity = context.User.Identity as ClaimsIdentity;
    //     IEnumerable<Claim> claims = identity!.Claims; 
    //     var claimId = identity.FindFirst("Id")?.Value;
    //     return Guid.Parse(claimId!);
    // }
}
