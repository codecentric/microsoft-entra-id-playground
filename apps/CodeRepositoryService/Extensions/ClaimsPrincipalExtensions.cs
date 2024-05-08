using System.Security.Claims;
using Microsoft.Identity.Web;

namespace CodeRepositoryService.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsInAnyRole(this ClaimsPrincipal claimsPrincipal, params string[] roles)
    {
        return roles.Any(claimsPrincipal.IsInRole);
    }

    public static bool HasAnyScope(this ClaimsPrincipal claimsPrincipal, params string[] scopes)
    {
        var scopeClaims = claimsPrincipal.FindAll(ClaimConstants.Scp)
            .Union(claimsPrincipal.FindAll(ClaimConstants.Scope))
            .ToList();

        return scopeClaims.SelectMany(s => s.Value.Split(' ')).Intersect(scopes).Any();
    }
}