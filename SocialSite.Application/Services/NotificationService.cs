using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Services;

// Oznámení se skládají z lajků a komentářů u mých příspěvků a z nových sledujících
public class NotificationService : INotificationService
{
    private const int Limit = 50;

    private readonly IRepository<Like> _likes;
    private readonly IRepository<Comment> _comments;
    private readonly IRepository<Follow> _follows;
    private readonly IRepository<UserProfile> _profiles;

    public NotificationService(IRepository<Like> likes, IRepository<Comment> comments,
        IRepository<Follow> follows, IRepository<UserProfile> profiles)
    {
        _likes = likes;
        _comments = comments;
        _follows = follows;
        _profiles = profiles;
    }

    public async Task<IReadOnlyList<NotificationDto>> GetForUserAsync(string userId)
    {
        var me = await _profiles.GetIdByUserIdAsync(userId);
        if (me is null) return [];

        var likes = await _likes.FindAsync(l => l.Post.UserProfileId == me && l.UserProfileId != me, [nameof(Like.UserProfile)]);
        var comments = await _comments.FindAsync(c => c.Post.UserProfileId == me && c.UserProfileId != me, [nameof(Comment.UserProfile)]);
        var follows = await _follows.FindAsync(f => f.ReceiverId == me, [nameof(Follow.Sender)]);

        return likes.Select(l => new NotificationDto
            {
                ActorId = l.UserProfileId,
                ActorName = l.UserProfile.Name,
                Message = "ohodnotil(a) váš příspěvek lajkem.",
                CreatedAt = l.CreatedAt,
                PostId = l.PostId
            })
            .Concat(comments.Select(c => new NotificationDto
            {
                ActorId = c.UserProfileId,
                ActorName = c.UserProfile.Name,
                Message = "přidal(a) komentář k vašemu příspěvku.",
                CreatedAt = c.CreatedAt,
                PostId = c.PostId
            }))
            .Concat(follows.Select(f => new NotificationDto
            {
                ActorId = f.SenderId,
                ActorName = f.Sender.Name,
                Message = "vás začal(a) sledovat.",
                CreatedAt = f.CreatedAt
            }))
            .OrderByDescending(n => n.CreatedAt)
            .Take(Limit)
            .ToList();
    }
}
