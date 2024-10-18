using MassTransit;
using Qwitter.BankAccounts.Contract.BankAccounts;
using Qwitter.BankAccounts.Contract.BankAccounts.Models;
using Qwitter.BankAccounts.Service.BankAccounts;
using Qwitter.BankAccounts.Service.User.Repositories;
using Qwitter.User.Contract.Events;
using Qwitter.User.Contract.User.Models;

namespace Qwitter.BankAccounts.Service.User.Consumers;

public class UserVerifiedConsumer : IConsumer<UserVerifiedEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly BankAccountService _bankAccountService;

    public UserVerifiedConsumer(
        IUserRepository userRepository,
        BankAccountService bankAccountService)
    {
        _userRepository = userRepository;
        _bankAccountService = bankAccountService;
    }

    public async Task Consume(ConsumeContext<UserVerifiedEvent> context)
    {
        var user = await _userRepository.GetById(context.Message.UserId);
        user.UserState = UserState.Verified;

        await _userRepository.Update(user);

        var bankAccount = await _bankAccountService.CreateBankAccount(new CreateBankAccountRequest
        {
            UserId = user.UserId,
            Currency = "USD"
        });

        await _bankAccountService.SetDefaultBankAccount(new SetDefaultBankAccountRequest
        {
            UserId = user.UserId,
            BankAccountId = bankAccount.BankAccountId
        });
    }
}