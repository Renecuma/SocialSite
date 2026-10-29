namespace SocialSite.Core.Entities;

public class Post : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? ImagePath { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}
