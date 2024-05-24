using MassTransit;
using Qwitter.Payments.User.Repositories;
using Qwitter.Users.Contract.User.Events;

namespace Qwitter.Payments.User.Consumers;

public class UserStateChangedConsumer : IConsumer<UserStateChangedEvent>
{
    private readonly ILogger<UserStateChangedConsumer> _logger;
    private readonly IUserRepository _userRepository;

    public UserStateChangedConsumer(
        ILogger<UserStateChangedConsumer> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserStateChangedEvent> context)
    {
        var user = await _userRepository.GetById(context.Message.UserId);

        if (user == null)
        {
            _logger.LogWarning("UserId {UserId} not found", context.Message.UserId);
            throw new Exception($"UserId {context.Message.UserId} not found");
        }

        user.UserState = context.Message.UserState;
        await _userRepository.Update(user);
    }
}