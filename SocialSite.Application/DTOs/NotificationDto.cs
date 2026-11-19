namespace SocialSite.Application.DTOs;

public class NotificationDto
{
    public int ActorId { get; set; }
    public string ActorName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int? PostId { get; set; }
}
