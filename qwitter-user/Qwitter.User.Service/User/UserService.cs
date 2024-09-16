
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.User.Contract.User;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.User.Service.User;

[ApiController]
[Route("user")]
public class UserService : ControllerBase, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("{userId}")]
    public async Task<UserResponse> GetUser(Guid userId)
    {
        var user = await _userRepository.GetById(userId);

        if (user == null)
        {
            throw new NotFoundApiException("User not found");
        }

        return new UserResponse
        {
            UserId = user.UserId,
            Email = user.Email
        };
    }
}