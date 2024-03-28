using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class RestApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public RestApiException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}