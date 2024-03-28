using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class BadRequestApiException : RestApiException
{
    public BadRequestApiException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}