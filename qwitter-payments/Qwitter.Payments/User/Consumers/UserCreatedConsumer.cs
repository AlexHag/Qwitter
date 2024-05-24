using MassTransit;
using Qwitter.Core.Application.Persistence;
using Qwitter.Payments.User.Models;
using Qwitter.Payments.User.Repositories;
using Qwitter.Users.Contract.User.Events;

namespace Qwitter.Payments.User.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IUserRepository _userRepository;

    public UserCreatedConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var user = new UserEntity
        {
            UserId = context.Message.UserId,
            Email = context.Message.Email,
            Username = context.Message.Username,
            UserState = UserState.Created,
            CreatedAt = DateTime.UtcNow,
        };

        await _userRepository.Insert(user);
    }
}