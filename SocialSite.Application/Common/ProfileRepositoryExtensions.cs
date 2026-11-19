using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Common;

internal static class ProfileRepositoryExtensions
{
    // Převede Id Identity uživatele na Id jeho profilu
    public static async Task<int?> GetIdByUserIdAsync(this IRepository<UserProfile> profiles, string? userId)
    {
        if (userId is null) return null;
        var profile = (await profiles.FindAsync(p => p.UserId == userId)).FirstOrDefault();
        return profile?.Id;
    }
}
