using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

public interface ILikeService
{
    Task ToggleAsync(int postId, string userId);
    Task<IReadOnlyList<LikeDto>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
}
