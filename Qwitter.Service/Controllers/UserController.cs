using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Service.Database.Repositories;
using Qwitter.Models.Requests;
using System.Security.Claims;

namespace Qwitter.Service.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public UserController(
        IConfiguration config,
        IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    [HttpPost]
    [Route("CreateAccount")]
    public async Task<IActionResult> CreateAccount(UsernamePasswordRequest request)
    {
        var newUser = await _userRepository.CreateUser(request.Username, request.Password);
        if (newUser is null) return BadRequest("Username already exists");

        var jwt = Helper.GenerateJwt(newUser.UserId, _config["Jwt:Key"]!, _config["Jwt:Issuer"]!, _config["Jwt:Audience"]!);
        return Ok(new { newUser.Username, newUser.IsPremium, jwt});
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(UsernamePasswordRequest request)
    {
        var user = await _userRepository.LoginUser(request.Username, request.Password);
        if (user is null) return BadRequest("Wrong username or password");

        var jwt = Helper.GenerateJwt(user.UserId, _config["Jwt:Key"]!, _config["Jwt:Issuer"]!, _config["Jwt:Audience"]!);
        return Ok(new { user.Username, user.IsPremium, jwt});
    }

    [HttpGet]
    [Route("Me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var claimId = identity!.FindFirst("Id")!.Value;
        var user = await _userRepository.GetUserById(Guid.Parse(claimId));
        if (user is null) return NotFound();
        
        return Ok(new { user.Username, user.IsPremium });
    }
}
