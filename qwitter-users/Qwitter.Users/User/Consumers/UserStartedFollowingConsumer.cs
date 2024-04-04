using MassTransit;
using Qwitter.Users.Contract.Follows.Events;
using Qwitter.Users.Repositories.User;

namespace Qwitter.Users.User.Consumers;

public class UserStartedFollowingConsumer : IConsumer<UserStartedFollowingEvent>
{
    private readonly IUserRepository _userRepository;

    public UserStartedFollowingConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserStartedFollowingEvent> context)
    {
        var followee = await _userRepository.GetUserById(context.Message.FolloweeId);
        var follower = await _userRepository.GetUserById(context.Message.FollowerId);
        
        if (followee is null)
        {
            Console.WriteLine($"WARNING: {context.Message.FolloweeId} started following {context.Message.FollowerId}, but folowee not found");
            return;
        }

        if (follower is null)
        {
            Console.WriteLine($"WARNING: {context.Message.FolloweeId} started following {context.Message.FollowerId}, but follower not found");
            return;
        }

        Console.WriteLine($"{followee.Username} started following {follower.Username}");
    }
}