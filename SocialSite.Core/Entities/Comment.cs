namespace SocialSite.Core.Entities;

public class Comment : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
}
