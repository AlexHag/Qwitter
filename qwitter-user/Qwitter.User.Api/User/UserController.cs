using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Authentication;
using Qwitter.User.Contract.User;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.User.Api.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public Task<UserResponse> GetUser()
    {
        return _userService.GetUser(User.Id());
    }
}