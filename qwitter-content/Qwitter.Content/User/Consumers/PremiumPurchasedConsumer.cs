using MassTransit;
using Qwitter.Content.Users.Models;
using Qwitter.Content.Users.Repositories;
using Qwitter.Core.Application.Kafka;
using Qwitter.Payments.Contract.Transactions.Events;

namespace Qwitter.Users.Premium.Consumers;

[MessageSuffix("premium")]
public class PremiumPurchasedConsumer : IConsumer<TransactionCompletedEvent>
{
    private readonly IUserRepository _userRepository;

    public PremiumPurchasedConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<TransactionCompletedEvent> context)
    {
        await _userRepository.UpdateUser(new UserUpdateModel
        {
            UserId = context.Message.UserId,
            HasPremium = true
        });
    }
}