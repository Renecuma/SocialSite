using Microsoft.AspNetCore.Hosting;
using SocialSite.Application.Interfaces;

namespace SocialSite.Infrastructure.Storage;

// Ukládá obrázky příspěvků i komentářů do wwwroot/uploads/posts pod náhodným názvem
public class LocalImageStorage : IImageStorage
{
    private const string PublicFolder = "/uploads/posts/";

    private readonly string _webRoot;
    private readonly string _folder;

    public LocalImageStorage(IWebHostEnvironment env)
    {
        _webRoot = env.WebRootPath;
        _folder = Path.Combine(_webRoot, "uploads", "posts");
    }

    public async Task<string> SaveAsync(Stream content, string extension)
    {
        Directory.CreateDirectory(_folder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        await using var file = File.Create(Path.Combine(_folder, fileName));
        await content.CopyToAsync(file);

        return PublicFolder + fileName;
    }

    public void Delete(string? path)
    {
        if (string.IsNullOrEmpty(path) || !path.StartsWith(PublicFolder)) return;

        // Path.GetFileName zabrání mazání mimo složku uploadů
        var fullPath = Path.Combine(_folder, Path.GetFileName(path));
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
