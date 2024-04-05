using MassTransit;
using Qwitter.Users.Contract.Follows.Events;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users.User.Consumers;

public class UserStartedFollowingConsumer : IConsumer<UserStartedFollowingEvent>
{
    private readonly ILogger<UserStartedFollowingConsumer> _logger;
    private readonly IUserRepository _userRepository;

    public UserStartedFollowingConsumer(
        ILogger<UserStartedFollowingConsumer> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserStartedFollowingEvent> context)
    {
        var followee = await _userRepository.GetUserById(context.Message.FolloweeId);
        var follower = await _userRepository.GetUserById(context.Message.FollowerId);
        
        if (followee is null)
        {
            _logger.LogWarning("{FolloweeId} started following {FollowerId}, but folowee not found", context.Message.FolloweeId, context.Message.FollowerId);
            return;
        }

        if (follower is null)
        {
            _logger.LogWarning("{FolloweeId} started following {FollowerId}, but follower not found", context.Message.FolloweeId, context.Message.FollowerId);
            return;
        }

        _logger.LogInformation("{FolloweeId} started following {FollowerId}", context.Message.FolloweeId, context.Message.FollowerId);
    }
}