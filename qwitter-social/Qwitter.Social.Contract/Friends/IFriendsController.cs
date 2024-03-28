using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Friends.Contract.Models;

namespace Qwitter.Social.Contract.Friends;

[ApiHost(Host.Port, "friends")]
public interface IFriendsController
{
    [HttpPost("send-friend-request/{username}")]
    Task SendFriendRequest(string username);

    [HttpGet("test")]
    Task<TestResponse> TestMethod(string testParam);
}