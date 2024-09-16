using System.Security.Claims;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Core.Application.Authentication;

public static class UserClaimsExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("id")!);
    }

    public static Guid Id(this ClaimsPrincipal user)
    {
        if (!Guid.TryParse(user.FindFirstValue("id"), out var userId))
        {
            throw new UnauthorizedApiException("Invalid user id");
        }

        return userId;
    }
}