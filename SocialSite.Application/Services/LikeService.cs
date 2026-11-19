using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Services;

public class LikeService : ILikeService
{
    private readonly IRepository<Like> _likes;
    private readonly IRepository<Post> _posts;
    private readonly IRepository<UserProfile> _profiles;

    public LikeService(IRepository<Like> likes, IRepository<Post> posts, IRepository<UserProfile> profiles)
    {
        _likes = likes;
        _posts = posts;
        _profiles = profiles;
    }

    public async Task ToggleAsync(int postId, string userId)
    {
        var profileId = await _profiles.GetIdByUserIdAsync(userId);
        if (profileId is null || !await _posts.ExistsAsync(p => p.Id == postId))
            return;

        var existing = (await _likes.FindAsync(l => l.PostId == postId && l.UserProfileId == profileId)).FirstOrDefault();
        if (existing is null)
            await _likes.AddAsync(new Like { PostId = postId, UserProfileId = profileId.Value });
        else
            _likes.Remove(existing);

        await _likes.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<LikeDto>> GetAllAsync()
    {
        var likes = await _likes.FindAsync(_ => true, [nameof(Like.UserProfile), nameof(Like.Post)]);
        return likes
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LikeDto
            {
                Id = l.Id,
                CreatedAt = l.CreatedAt,
                PostId = l.PostId,
                PostExcerpt = l.Post.Content.Length > 60 ? l.Post.Content[..60] + "…" : l.Post.Content,
                UserName = l.UserProfile.Name
            })
            .ToList();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var like = await _likes.GetByIdAsync(id);
        if (like is null) return false;

        _likes.Remove(like);
        await _likes.SaveChangesAsync();
        return true;
    }
}
