namespace Qwitter.Users.Exceptions;

public class UsernameAlreadyExistException : Exception
{
    public UsernameAlreadyExistException(string message) : base(message) { }
    public UsernameAlreadyExistException() { }
}