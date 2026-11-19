using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

public interface IFollowService
{
    Task ToggleAsync(int receiverProfileId, string userId);
    Task<IReadOnlyList<FollowDto>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
}
