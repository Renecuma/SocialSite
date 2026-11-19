using System.Security.Claims;

namespace SocialSite.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    // Id přihlášeného uživatele (AspNetUsers.Id), null pro nepřihlášené
    public static string? GetUserId(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier);
}
