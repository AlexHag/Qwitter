using MassTransit;
using Qwitter.Content.Users.Models;
using Qwitter.Content.Users.Repositories;
using Qwitter.Users.Contract.User.Events;

namespace Qwitter.Content.Users.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IUserRepository _userRepository;

    public UserCreatedConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        await _userRepository.InsertUser(new UserInsertModel
        {
            UserId = context.Message.UserId,
            Username = context.Message.Username
        });
    }
}