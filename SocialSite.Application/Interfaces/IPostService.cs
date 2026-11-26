using SocialSite.Application.Common;
using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

// ownerUserId: pokud je zadán, operace je povolena jen autorovi; null = bez kontroly (admin/manager)
public interface IPostService
{
    Task<IReadOnlyList<PostDto>> GetFeedAsync(string? currentUserId);
    Task<PostDto?> GetDetailAsync(int id, string? currentUserId);
    Task<PostInputDto?> GetForEditAsync(int id, string? ownerUserId);
    Task<int> CreateAsync(PostInputDto input, ImageUpload? image, string userId);
    Task<bool> UpdateAsync(int id, PostInputDto input, ImageUpload? image, string? ownerUserId);
    Task<bool> DeleteAsync(int id, string? ownerUserId);
}
