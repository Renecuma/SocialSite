using SocialSite.Application.Common;
using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

// Implementace je v Infrastructure (ASP.NET Identity)
public interface IUserAdminService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync();
    Task<UserEditDto?> GetForEditAsync(string id);
    Task<OperationResult> UpdateAsync(UserEditDto input);
    Task<OperationResult> DeleteAsync(string id, string currentUserId);
}
