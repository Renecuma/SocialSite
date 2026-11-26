using SocialSite.Application.Common;
using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

public interface ICommentService
{
    Task<IReadOnlyList<CommentDto>> GetAllAsync();
    Task<bool> AddAsync(CommentInputDto input, ImageUpload? image, string userId);
    Task<CommentInputDto?> GetForEditAsync(int id);
    Task<bool> UpdateAsync(int id, CommentInputDto input);
    // Vrací Id příspěvku, ke kterému komentář patřil, nebo null, pokud smazání neproběhlo
    Task<int?> DeleteAsync(int id, string? ownerUserId);
}
