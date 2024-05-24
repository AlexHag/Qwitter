namespace Qwitter.Core.Application.Persistence;

public enum UserState
{
    Unknown = 0,
    Created = 1,
    Verified = 10,
    Canceled = 20,
    Blocked = 30
}