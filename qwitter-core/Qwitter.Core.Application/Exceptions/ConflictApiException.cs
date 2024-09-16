using System.Net;

namespace Qwitter.Core.Application.Exceptions;

public class ConflictApiException : RestApiException
{
    public ConflictApiException(string message) : base(message, HttpStatusCode.Conflict)
    {
    }
}