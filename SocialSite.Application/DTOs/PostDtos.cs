using System.ComponentModel.DataAnnotations;

namespace SocialSite.Application.DTOs;

public class PostDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;

    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool LikedByMe { get; set; }
    public bool IsMine { get; set; }

    public IReadOnlyList<CommentDto> Comments { get; set; } = [];
}

public class PostInputDto
{
    [Required(ErrorMessage = "Obsah příspěvku je povinný.")]
    [StringLength(2000, ErrorMessage = "Příspěvek může mít maximálně {1} znaků.")]
    [Display(Name = "Obsah příspěvku")]
    public string Content { get; set; } = string.Empty;
}
