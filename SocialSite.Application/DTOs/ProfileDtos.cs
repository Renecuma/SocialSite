using System.ComponentModel.DataAnnotations;
using SocialSite.Application.Validation;

namespace SocialSite.Application.DTOs;

public class ProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public bool IsMe { get; set; }
    public bool IsFollowedByMe { get; set; }

    public IReadOnlyList<PostDto> Posts { get; set; } = [];
}

public class ProfileEditDto
{
    [Required(ErrorMessage = "Jméno je povinné.")]
    [StringLength(100, ErrorMessage = "Jméno může mít maximálně {1} znaků.")]
    [Display(Name = "Jméno")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Bio může mít maximálně {1} znaků.")]
    [Display(Name = "Bio")]
    public string? Bio { get; set; }

    [Required(ErrorMessage = "Datum narození je povinné.")]
    [MinimumAge(13)]
    [DataType(DataType.Date)]
    [Display(Name = "Datum narození")]
    public DateOnly? DateOfBirth { get; set; }
}
