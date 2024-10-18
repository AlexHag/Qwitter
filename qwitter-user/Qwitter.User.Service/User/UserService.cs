using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;
using Qwitter.User.Contract.Events;
using Qwitter.User.Contract.User;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.User.Service.User;

[ApiController]
[Route("user")]
public class UserService : ControllerBase, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEventProducer _eventProducer;
    private readonly IMapper _mapper;

    public UserService(
        IUserRepository userRepository,
        IEventProducer eventProducer,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _eventProducer = eventProducer;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
    public async Task<UserResponse> GetUser(Guid userId)
    {
        var user = await _userRepository.TryGetById(userId);

        if (user == null)
        {
            throw new NotFoundApiException("User not found");
        }

        return _mapper.Map<UserResponse>(user);
    }

    [HttpPut("{userId}/verify")]
    public async Task<UserResponse> VerifyUser(Guid userId)
    {
        var user = await _userRepository.TryGetById(userId);

        if (user == null)
        {
            throw new NotFoundApiException("User not found");
        }

        if (user.UserState != UserState.Unverified)
        {
            throw new BadRequestApiException($"Cannot verify user in state {user.UserState}");
        }

        user.UserState = UserState.Verified;
        await _userRepository.Update(user);

        await _eventProducer.Produce(new UserVerifiedEvent { UserId = userId });

        return _mapper.Map<UserResponse>(user);
    }
}