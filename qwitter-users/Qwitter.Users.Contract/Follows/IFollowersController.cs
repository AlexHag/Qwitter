using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Users.Contract.Follows.Models;
using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.Contract.Follows;

[ApiHost(Host.Port, "follows")]
public interface IFollowsController
{
    [HttpPost("start-following")]
    Task StartFollowing(StartFollowingRequest request);

    [HttpPost("stop-following")]
    Task StopFollowing(StopFollowingRequest request);

    [HttpPost("followers")]
    Task<IEnumerable<UserPublicProfile>> GetFollowers();
}
