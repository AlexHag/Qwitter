using System.Security.Claims;

namespace Qwitter.Core.Application.Authentication;

public static class UserClaimsExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("id")!);
    }
}