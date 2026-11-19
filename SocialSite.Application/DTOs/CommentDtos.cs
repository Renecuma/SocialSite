using System.ComponentModel.DataAnnotations;

namespace SocialSite.Application.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public int PostId { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public bool IsMine { get; set; }
}

public class CommentInputDto
{
    public int PostId { get; set; }

    [Required(ErrorMessage = "Text komentáře je povinný.")]
    [StringLength(1000, ErrorMessage = "Komentář může mít maximálně {1} znaků.")]
    [Display(Name = "Komentář")]
    public string Text { get; set; } = string.Empty;
}
