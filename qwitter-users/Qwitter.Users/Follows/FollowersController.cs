using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Authentication;
using Qwitter.Core.Application.Kafka;
using Qwitter.Users.Contract.Follows;
using Qwitter.Users.Contract.Follows.Events;
using Qwitter.Users.Contract.Follows.Models;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.Follows.Repositories;

namespace Qwitter.Users.Follows;

[ApiController]
[Route("follows")]
public class FollowersController : ControllerBase, IFollowsController
{
    private readonly IFollowsRepository _followersRepository;
    private readonly IEventProducer _eventProducer;

    public FollowersController(
        IFollowsRepository followersRepository,
        IEventProducer eventProducer)
    {
        _followersRepository = followersRepository;
        _eventProducer = eventProducer;
    }

    // TODO: Add pagination
    [HttpPost("followers")]
    public async Task<IEnumerable<UserPublicProfile>> GetFollowers()
    {
        return await _followersRepository.GetFollowers(User.GetUserId());
    }

    [HttpPost("start-following")]
    public async Task StartFollowing(StartFollowingRequest request)
    {
        await _followersRepository.StartFollowing(User.GetUserId(), request.UserId);
        await _eventProducer.Produce(new UserStartedFollowingEvent
        {
            FolloweeId = User.GetUserId(),
            FollowerId = request.UserId
        });
    }
    
    [HttpPost("stop-following")]
    public async Task StopFollowing(StopFollowingRequest request)
    {
        await _followersRepository.StopFollowing(User.GetUserId(), request.UserId);
        await _eventProducer.Produce(new UserStoppedFollowingEvent
        {
            FolloweeId = User.GetUserId(),
            FollowerId = request.UserId
        });
    }
}
