using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Services;

public class PostService : IPostService
{
    private static readonly string[] ListIncludes = [nameof(Post.UserProfile), nameof(Post.Likes), nameof(Post.Comments)];

    private readonly IRepository<Post> _posts;
    private readonly IRepository<UserProfile> _profiles;
    private readonly IImageStorage _images;

    public PostService(IRepository<Post> posts, IRepository<UserProfile> profiles, IImageStorage images)
    {
        _posts = posts;
        _profiles = profiles;
        _images = images;
    }

    public async Task<IReadOnlyList<PostDto>> GetFeedAsync(string? currentUserId)
    {
        var myProfileId = await _profiles.GetIdByUserIdAsync(currentUserId);
        var posts = await _posts.FindAsync(_ => true, ListIncludes);

        return posts
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => PostMapper.ToDto(p, myProfileId))
            .ToList();
    }

    public async Task<PostDto?> GetDetailAsync(int id, string? currentUserId)
    {
        var myProfileId = await _profiles.GetIdByUserIdAsync(currentUserId);
        var post = await _posts.FirstOrDefaultAsync(p => p.Id == id,
            [.. ListIncludes, $"{nameof(Post.Comments)}.{nameof(Comment.UserProfile)}"]);
        if (post is null) return null;

        var dto = PostMapper.ToDto(post, myProfileId);
        dto.Comments = post.Comments
            .OrderBy(c => c.CreatedAt)
            .Select(c => PostMapper.ToDto(c, myProfileId))
            .ToList();
        return dto;
    }

    public async Task<PostInputDto?> GetForEditAsync(int id, string? ownerUserId)
    {
        var post = await GetAuthorizedAsync(id, ownerUserId);
        return post is null ? null : new PostInputDto { Content = post.Content, ImagePath = post.ImagePath };
    }

    public async Task<int> CreateAsync(PostInputDto input, ImageUpload? image, string userId)
    {
        var profileId = await _profiles.GetIdByUserIdAsync(userId)
            ?? throw new InvalidOperationException("Uživatel nemá profil.");

        var post = new Post { Content = input.Content.Trim(), UserProfileId = profileId };
        if (image is not null)
            post.ImagePath = await _images.SaveAsync(image.Content, image.Extension);

        await _posts.AddAsync(post);
        await _posts.SaveChangesAsync();
        return post.Id;
    }

    public async Task<bool> UpdateAsync(int id, PostInputDto input, ImageUpload? image, string? ownerUserId)
    {
        var post = await GetAuthorizedAsync(id, ownerUserId);
        if (post is null) return false;

        // Nový obrázek nahradí starý; starý soubor se smaže až po uložení změn
        var oldImage = post.ImagePath;
        if (image is not null)
            post.ImagePath = await _images.SaveAsync(image.Content, image.Extension);
        else if (input.RemoveImage)
            post.ImagePath = null;

        post.Content = input.Content.Trim();
        post.UpdatedAt = DateTime.UtcNow;
        _posts.Update(post);
        await _posts.SaveChangesAsync();

        if (oldImage != post.ImagePath)
            _images.Delete(oldImage);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string? ownerUserId)
    {
        var post = await GetAuthorizedAsync(id, ownerUserId);
        if (post is null) return false;

        _posts.Remove(post);
        await _posts.SaveChangesAsync();
        _images.Delete(post.ImagePath);
        return true;
    }

    private async Task<Post?> GetAuthorizedAsync(int id, string? ownerUserId)
    {
        var post = await _posts.GetByIdAsync(id);
        if (post is null || ownerUserId is null) return post;

        var profileId = await _profiles.GetIdByUserIdAsync(ownerUserId);
        return post.UserProfileId == profileId ? post : null;
    }
}
