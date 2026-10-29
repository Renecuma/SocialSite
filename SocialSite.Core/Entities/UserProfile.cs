namespace SocialSite.Core.Entities;

public class UserProfile : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePicturePath { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();

    // Koho sleduji (já jsem Sender)
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
    // Kdo sleduje mě (já jsem Receiver)
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
}
