using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.Contract.User;

[ApiHost(Host.Port, "user")]
public interface IUserController
{
    [HttpGet("me")]
    Task<UserProfile> GetUser();

    [HttpPut("{userId}/verify")]
    Task VerifyUser(Guid userId);

    [HttpPut("{userId}/cancel")]
    Task CancelUser(Guid userId);

    [HttpPut("{userId}/block")]
    Task BlockUser(Guid userId);

    [HttpPut("{userId}/unblock")]
    Task UnBlockUser(Guid userId);
}
