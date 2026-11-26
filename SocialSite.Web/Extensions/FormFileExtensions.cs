using Microsoft.AspNetCore.Mvc.ModelBinding;
using SocialSite.Application.Common;

namespace SocialSite.Web.Extensions;

public static class FormFileExtensions
{
    // Převede nahraný soubor na ImageUpload; chybu zapíše do ModelState pod klíč "Image"
    public static ImageUpload? ToImageUpload(this IFormFile? file, ModelStateDictionary modelState)
    {
        if (file is null) return null;

        var upload = new ImageUpload(file.OpenReadStream(), file.FileName, file.Length);
        if (upload.Validate() is { } error)
            modelState.AddModelError("Image", error);
        return upload;
    }
}
