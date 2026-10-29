namespace SocialSite.Core.Entities;

public class Follow : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int SenderId { get; set; }
    public UserProfile Sender { get; set; } = null!;

    public int ReceiverId { get; set; }
    public UserProfile Receiver { get; set; } = null!;
}
