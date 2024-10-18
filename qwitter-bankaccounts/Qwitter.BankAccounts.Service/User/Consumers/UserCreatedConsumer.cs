using MassTransit;
using Qwitter.BankAccounts.Service.User.Models;
using Qwitter.BankAccounts.Service.User.Repositories;
using Qwitter.User.Contract.Events;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.BankAccounts.Service.User.Consumers;

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
            UserState = UserState.Unverified
        };

        await _userRepository.Insert(user);
    }
}