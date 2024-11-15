using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class ForbiddenApiException : RestApiException
{
    public ForbiddenApiException(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }
}