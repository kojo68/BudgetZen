using System.Security.Claims;

namespace BudgetZen.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue(ClaimTypes.Name) ?? user.FindFirstValue("sub");
        return sub is null ? Guid.Empty : Guid.Parse(sub);
    }
}
