using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class InternalServerErrorApiException : RestApiException
{
    public InternalServerErrorApiException(string message) : base(message, HttpStatusCode.InternalServerError)
    {
    }
}