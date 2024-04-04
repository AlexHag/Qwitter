using MassTransit;
using Qwitter.Users.Contract.Follows.Events;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users.User.Consumers;

public class UserStoppedFollowingConsumer : IConsumer<UserStoppedFollowingEvent>
{
    private readonly IUserRepository _userRepository;

    public UserStoppedFollowingConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserStoppedFollowingEvent> context)
    {
        var followee = await _userRepository.GetUserById(context.Message.FolloweeId);
        var follower = await _userRepository.GetUserById(context.Message.FollowerId);
        
        if (followee is null)
        {
            Console.WriteLine($"WARNING: {context.Message.FolloweeId} stopped following {context.Message.FollowerId}, but folowee not found");
            return;
        }

        if (follower is null)
        {
            Console.WriteLine($"WARNING: {context.Message.FolloweeId} stopped following {context.Message.FollowerId}, but follower not found");
            return;
        }

        Console.WriteLine($"{followee.Username} stopped following {follower.Username}");
    }
}