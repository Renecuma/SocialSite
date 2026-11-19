using SocialSite.Application.DTOs;

namespace SocialSite.Application.Interfaces;

public interface INotificationService
{
    Task<IReadOnlyList<NotificationDto>> GetForUserAsync(string userId);
}
