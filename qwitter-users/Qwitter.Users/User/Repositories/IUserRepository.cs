using Qwitter.Users.User.Models;

namespace Qwitter.Users.Repositories.User;

public interface IUserRepository
{
    Task<UserEntity?> GetUserById(Guid userId);
    Task<UserEntity?> GetUserByUsername(string username);
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity?> GetUserByUsernameOrEmail(string usernameOrEmail);
    Task<UserEntity> InsertUser(UserInsertModel user);
    Task<UserEntity> UpdateUser(UserUpdateModel user);
    Task<UserEntity> DeleteUser(Guid userId);
}