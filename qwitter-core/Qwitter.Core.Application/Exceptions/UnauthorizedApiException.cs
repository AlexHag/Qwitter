using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class UnauthorizedApiException : RestApiException
{
    public UnauthorizedApiException(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}