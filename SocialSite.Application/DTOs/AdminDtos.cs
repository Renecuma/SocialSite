using System.ComponentModel.DataAnnotations;
using SocialSite.Application.Validation;

namespace SocialSite.Application.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? ProfileId { get; set; }
}

// Admin může upravit údaje uživatele, ale ne heslo
public class UserEditDto
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Neplatný formát e-mailu.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

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

    [Required(ErrorMessage = "Role je povinná.")]
    [Display(Name = "Role")]
    public string Role { get; set; } = string.Empty;
}

public class LikeDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PostId { get; set; }
    public string PostExcerpt { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}

public class FollowDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
}
