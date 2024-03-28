using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class NotFoundApiException : RestApiException
{
    public NotFoundApiException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}