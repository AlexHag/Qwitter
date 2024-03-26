using Qwitter.Users.Contract.User.Models;

namespace Qwitter.Users.Contract.User;

public interface IUserController
{
    Task<UserProfile> GetUser();
}
