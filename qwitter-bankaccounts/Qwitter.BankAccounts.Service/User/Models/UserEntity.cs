using Qwitter.User.Contract.User.Models;

namespace Qwitter.BankAccounts.Service.User.Models;

public class UserEntity
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public UserState UserState { get; set; }
}