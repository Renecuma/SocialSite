namespace SocialSite.Application.Interfaces;

// Úložiště nahraných obrázků; vrací veřejnou cestu (např. /uploads/posts/abc.jpg)
public interface IImageStorage
{
    Task<string> SaveAsync(Stream content, string extension);
    void Delete(string? path);
}
