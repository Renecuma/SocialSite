using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Services;

public class FollowService : IFollowService
{
    private readonly IRepository<Follow> _follows;
    private readonly IRepository<UserProfile> _profiles;

    public FollowService(IRepository<Follow> follows, IRepository<UserProfile> profiles)
    {
        _follows = follows;
        _profiles = profiles;
    }

    public async Task ToggleAsync(int receiverProfileId, string userId)
    {
        var senderId = await _profiles.GetIdByUserIdAsync(userId);
        if (senderId is null || senderId == receiverProfileId || !await _profiles.ExistsAsync(p => p.Id == receiverProfileId))
            return;

        var existing = (await _follows.FindAsync(f => f.SenderId == senderId && f.ReceiverId == receiverProfileId)).FirstOrDefault();
        if (existing is null)
            await _follows.AddAsync(new Follow { SenderId = senderId.Value, ReceiverId = receiverProfileId });
        else
            _follows.Remove(existing);

        await _follows.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<FollowDto>> GetAllAsync()
    {
        var follows = await _follows.FindAsync(_ => true, [nameof(Follow.Sender), nameof(Follow.Receiver)]);
        return follows
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FollowDto
            {
                Id = f.Id,
                CreatedAt = f.CreatedAt,
                SenderName = f.Sender.Name,
                ReceiverName = f.Receiver.Name
            })
            .ToList();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var follow = await _follows.GetByIdAsync(id);
        if (follow is null) return false;

        _follows.Remove(follow);
        await _follows.SaveChangesAsync();
        return true;
    }
}
