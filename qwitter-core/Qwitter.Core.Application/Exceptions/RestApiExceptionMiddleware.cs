
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Qwitter.Core.Application.Exceptions;

public class RestApiExceptionMiddleware
{
    // TODO: Add logger
    private readonly RequestDelegate _next;

    public RestApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = exception switch
        {
            BadRequestApiException _ => HttpStatusCode.BadRequest,
            NotFoundApiException _ => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError,
        };

        var result = JsonSerializer.Serialize(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}

