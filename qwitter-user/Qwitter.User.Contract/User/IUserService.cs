using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.User.Contract.User;

[ApiHost(Host.Name, "user")]
public interface IUserService
{
    [HttpGet("{userId}")]
    Task<UserResponse> GetUser(Guid userId);

    [HttpPut("{userId}/verify")]
    Task<UserResponse> VerifyUser(Guid userId);
}