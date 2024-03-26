using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Contract.User;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase, IUserController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [Authorize]
    [HttpGet("me")]
    public Task<UserProfile> GetUser()
    {
        // var user = _userRepository.GetUserById(User.GetUserId());
        throw new NotImplementedException();
    }
}