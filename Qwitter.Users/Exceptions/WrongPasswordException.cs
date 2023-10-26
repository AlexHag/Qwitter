namespace Qwitter.Users.Exceptions;

public class LoginFailedException : Exception
{
    public LoginFailedException(string message) : base(message) { }
}
