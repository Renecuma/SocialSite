using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

public interface IProfileService
{
    // profileId null = profil přihlášeného uživatele
    Task<ProfileDto?> GetAsync(int? profileId, string? currentUserId);
    Task<ProfileEditDto?> GetForEditAsync(string userId);
    Task<bool> UpdateAsync(string userId, ProfileEditDto input);
}
