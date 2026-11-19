using SocialSite.Application.Common;
using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

// Implementace je v Infrastructure (ASP.NET Identity)
public interface IAccountService
{
    Task<OperationResult> RegisterAsync(RegisterDto input);
    Task<bool> LoginAsync(LoginDto input);
    Task LogoutAsync();
}
