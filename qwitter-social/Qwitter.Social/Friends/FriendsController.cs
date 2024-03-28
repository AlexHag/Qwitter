using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Authentication;
using Qwitter.Friends.Contract.Models;
using Qwitter.Social.Contract.Friends;
using Qwitter.Users.Contract.User;

namespace Qwitter.Social.Friends;

[ApiController]
[Route("friends")]
public class FriendsController : ControllerBase, IFriendsController
{
    private readonly IUserController _userController;

    public FriendsController(IUserController userController)
    {
        _userController = userController;
    }

    [HttpPost("send-friend-request/{username}")]
    public Task SendFriendRequest(string username)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpGet("test")]
    public async Task<TestResponse> TestMethod(string testParam)
    {
        var response = await _userController.Get("This ", "is from firends", testParam, "controller");
        var postResponse = await _userController.Post("This", "is from firends", "controller", new Users.Contract.User.Models.TestRequest { Message = $"Hello UserId: {User.GetUserId()}", Number = 42 });
        return new TestResponse { Message = $"{response.Message} - {postResponse.Message}" };
    }
}