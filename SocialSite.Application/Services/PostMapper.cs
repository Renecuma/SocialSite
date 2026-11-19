using SocialSite.Application.DTOs;
using SocialSite.Core.Entities;

namespace SocialSite.Application.Services;

internal static class PostMapper
{
    // Očekává načtené navigace UserProfile, Likes a Comments
    public static PostDto ToDto(Post post, int? myProfileId) => new()
    {
        Id = post.Id,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        UpdatedAt = post.UpdatedAt,
        AuthorId = post.UserProfileId,
        AuthorName = post.UserProfile.Name,
        LikeCount = post.Likes.Count,
        CommentCount = post.Comments.Count,
        LikedByMe = post.Likes.Any(l => l.UserProfileId == myProfileId),
        IsMine = post.UserProfileId == myProfileId
    };

    public static CommentDto ToDto(Comment comment, int? myProfileId) => new()
    {
        Id = comment.Id,
        Text = comment.Text,
        CreatedAt = comment.CreatedAt,
        PostId = comment.PostId,
        AuthorId = comment.UserProfileId,
        AuthorName = comment.UserProfile.Name,
        IsMine = comment.UserProfileId == myProfileId
    };
}
