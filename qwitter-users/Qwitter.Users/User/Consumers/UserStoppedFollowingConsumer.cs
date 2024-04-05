using MassTransit;
using Qwitter.Users.Contract.Follows.Events;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users.User.Consumers;

public class UserStoppedFollowingConsumer : IConsumer<UserStoppedFollowingEvent>
{
    private readonly ILogger<UserStoppedFollowingConsumer> _logger;
    private readonly IUserRepository _userRepository;

    public UserStoppedFollowingConsumer(
        ILogger<UserStoppedFollowingConsumer> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserStoppedFollowingEvent> context)
    {
        var followee = await _userRepository.GetUserById(context.Message.FolloweeId);
        var follower = await _userRepository.GetUserById(context.Message.FollowerId);
        
        if (followee is null)
        {
            _logger.LogWarning("{FolloweeId} stopped following {FollowerId}, but folowee not found", context.Message.FolloweeId, context.Message.FollowerId);
            return;
        }

        if (follower is null)
        {
            _logger.LogWarning("{FolloweeId} stopped following {FollowerId}, but follower not found", context.Message.FolloweeId, context.Message.FollowerId);
            return;
        }

        _logger.LogInformation("{FolloweeId} stopped following {FollowerId}", context.Message.FolloweeId, context.Message.FollowerId);
    }
}