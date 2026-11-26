using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Services;

public class CommentService : ICommentService
{
    private readonly IRepository<Comment> _comments;
    private readonly IRepository<Post> _posts;
    private readonly IRepository<UserProfile> _profiles;
    private readonly IImageStorage _images;

    public CommentService(IRepository<Comment> comments, IRepository<Post> posts, IRepository<UserProfile> profiles,
        IImageStorage images)
    {
        _comments = comments;
        _posts = posts;
        _profiles = profiles;
        _images = images;
    }

    public async Task<IReadOnlyList<CommentDto>> GetAllAsync()
    {
        var comments = await _comments.FindAsync(_ => true, [nameof(Comment.UserProfile)]);
        return comments
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => PostMapper.ToDto(c, null))
            .ToList();
    }

    public async Task<bool> AddAsync(CommentInputDto input, ImageUpload? image, string userId)
    {
        var profileId = await _profiles.GetIdByUserIdAsync(userId);
        if (profileId is null || !await _posts.ExistsAsync(p => p.Id == input.PostId))
            return false;

        await _comments.AddAsync(new Comment
        {
            PostId = input.PostId,
            UserProfileId = profileId.Value,
            Text = input.Text.Trim(),
            ImagePath = image is null ? null : await _images.SaveAsync(image.Content, image.Extension)
        });
        await _comments.SaveChangesAsync();
        return true;
    }

    public async Task<CommentInputDto?> GetForEditAsync(int id)
    {
        var comment = await _comments.GetByIdAsync(id);
        return comment is null ? null : new CommentInputDto { PostId = comment.PostId, Text = comment.Text, ImagePath = comment.ImagePath };
    }

    public async Task<bool> UpdateAsync(int id, CommentInputDto input)
    {
        var comment = await _comments.GetByIdAsync(id);
        if (comment is null) return false;

        var oldImage = comment.ImagePath;
        if (input.RemoveImage)
            comment.ImagePath = null;

        comment.Text = input.Text.Trim();
        _comments.Update(comment);
        await _comments.SaveChangesAsync();

        if (oldImage != comment.ImagePath)
            _images.Delete(oldImage);
        return true;
    }

    public async Task<int?> DeleteAsync(int id, string? ownerUserId)
    {
        var comment = await _comments.GetByIdAsync(id);
        if (comment is null) return null;

        if (ownerUserId is not null && comment.UserProfileId != await _profiles.GetIdByUserIdAsync(ownerUserId))
            return null;

        _comments.Remove(comment);
        await _comments.SaveChangesAsync();
        _images.Delete(comment.ImagePath);
        return comment.PostId;
    }
}
