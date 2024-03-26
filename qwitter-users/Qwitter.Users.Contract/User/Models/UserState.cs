namespace Qwitter.Users.Contract.User.Models;

public enum UserState
{
    Unknown = 0,
    Created = 1,
    Verified,
    Blocked,
    Deleted
}