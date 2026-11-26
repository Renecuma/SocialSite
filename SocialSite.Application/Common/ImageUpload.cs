namespace SocialSite.Application.Common;

// Nahraný obrázek nezávislý na ASP.NET (IFormFile se převádí v kontroleru)
public record ImageUpload(Stream Content, string FileName, long Length)
{
    public const long MaxBytes = 5 * 1024 * 1024;
    public static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    public string Extension => Path.GetExtension(FileName).ToLowerInvariant();

    // Vrací chybovou hlášku, nebo null, pokud je soubor v pořádku
    public string? Validate()
    {
        if (Length == 0)
            return "Soubor je prázdný.";
        if (Length > MaxBytes)
            return $"Obrázek může mít maximálně {MaxBytes / 1024 / 1024} MB.";
        if (!AllowedExtensions.Contains(Extension))
            return "Povolené formáty jsou JPG, PNG, GIF a WEBP.";
        return null;
    }
}
