using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Users.Contract.User;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.Repositories.User;
using Qwitter.Core.Application.Authentication;
using Qwitter.Core.Application.Persistence;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Users.User.Models;
using Qwitter.Core.Application.Kafka;
using Qwitter.Users.Contract.User.Events;

namespace Qwitter.Users.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase, IUserController
{
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IEventProducer _eventProducer;

    public UserController(
        ILogger<UserController> logger,
        IMapper mapper,
        IUserRepository userRepository,
        IEventProducer eventProducer)
    {
        _mapper = mapper;
        _logger = logger;
        _userRepository = userRepository;
        _eventProducer = eventProducer;
    }

    [HttpPut("{userId}/block")]
    public Task BlockUser([FromRoute] Guid userId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{userId}/cancel")]
    public async Task CancelUser([FromRoute] Guid userId)
    {
        var user = await _userRepository.GetUserById(userId) ?? throw new NotFoundApiException($"User with id {userId} not found");

        if (user.UserState == UserState.Canceled)
        {
            _logger.LogWarning("Cancelling user that is already canceled. {user.UserId}", user.UserId);
            return;
        }

        await _userRepository.UpdateUser(new UserUpdateModel
        {
            UserId = userId,
            UserState = UserState.Canceled
        });

        await _eventProducer.Produce(new UserStateChangedEvent
        {
            UserId = userId,
            UserState = UserState.Canceled
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<UserProfile> GetUser()
    {
        var user = await _userRepository.GetUserById(User.GetUserId());
        return _mapper.Map<UserProfile>(user!);
    }

    [HttpPut("{userId}/unblock")]
    public async Task UnBlockUser([FromRoute] Guid userId)
    {
        var user = await _userRepository.GetUserById(userId) ?? throw new NotFoundApiException($"User with id {userId} not found");

        if (user.UserState != UserState.Blocked)
        {
            throw new InvalidOperationException($"Cannot unblock user that is not blocked. User state is: {user.UserState}");
        }

        await _userRepository.UpdateUser(new UserUpdateModel
        {
            UserId = userId,
            UserState = UserState.Verified
        });

        await _eventProducer.Produce(new UserStateChangedEvent
        {
            UserId = userId,
            UserState = UserState.Verified
        });
    }

    [HttpPut("{userId}/verify")]
    public async Task VerifyUser(Guid userId)
    {
        var user = await _userRepository.GetUserById(userId) ?? throw new NotFoundApiException($"User with id {userId} not found");

        if (user.UserState != UserState.Created)
        {
            throw new InvalidOperationException($"Cannot verify user in state {user.UserState}");
        }

        await _userRepository.UpdateUser(new UserUpdateModel
        {
            UserId = userId,
            UserState = UserState.Verified
        });

        await _eventProducer.Produce(new UserStateChangedEvent
        {
            UserId = userId,
            UserState = UserState.Verified
        });
    }
}