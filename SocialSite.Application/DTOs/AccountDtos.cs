using System.ComponentModel.DataAnnotations;
using SocialSite.Application.Validation;

namespace SocialSite.Application.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Neplatný formát e-mailu.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jméno je povinné.")]
    [StringLength(100, ErrorMessage = "Jméno může mít maximálně {1} znaků.")]
    [Display(Name = "Jméno")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Datum narození je povinné.")]
    [MinimumAge(13)]
    [DataType(DataType.Date)]
    [Display(Name = "Datum narození")]
    public DateOnly? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Heslo je povinné.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Heslo musí mít {2} až {1} znaků.")]
    [DataType(DataType.Password)]
    [Display(Name = "Heslo")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
    [Compare(nameof(Password), ErrorMessage = "Hesla se neshodují.")]
    [DataType(DataType.Password)]
    [Display(Name = "Potvrzení hesla")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class LoginDto
{
    [Required(ErrorMessage = "E-mail je povinný.")]
    [EmailAddress(ErrorMessage = "Neplatný formát e-mailu.")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné.")]
    [DataType(DataType.Password)]
    [Display(Name = "Heslo")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Zapamatovat si mě")]
    public bool RememberMe { get; set; }
}
