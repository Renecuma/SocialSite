namespace SocialSite.Core.Entities;

public class Like : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
}
