using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IRepository<UserProfile> _profiles;
    private readonly IRepository<Post> _posts;

    public ProfileService(IRepository<UserProfile> profiles, IRepository<Post> posts)
    {
        _profiles = profiles;
        _posts = posts;
    }

    public async Task<ProfileDto?> GetAsync(int? profileId, string? currentUserId)
    {
        var myProfileId = await _profiles.GetIdByUserIdAsync(currentUserId);
        var id = profileId ?? myProfileId;
        if (id is null) return null;

        var profile = await _profiles.FirstOrDefaultAsync(p => p.Id == id,
            [nameof(UserProfile.Followers), nameof(UserProfile.Following)]);
        if (profile is null) return null;

        var posts = await _posts.FindAsync(p => p.UserProfileId == profile.Id,
            [nameof(Post.UserProfile), nameof(Post.Likes), nameof(Post.Comments)]);

        return new ProfileDto
        {
            Id = profile.Id,
            Name = profile.Name,
            Bio = profile.Bio,
            DateOfBirth = profile.DateOfBirth,
            FollowersCount = profile.Followers.Count,
            FollowingCount = profile.Following.Count,
            IsMe = profile.Id == myProfileId,
            IsFollowedByMe = profile.Followers.Any(f => f.SenderId == myProfileId),
            Posts = posts
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => PostMapper.ToDto(p, myProfileId))
                .ToList()
        };
    }

    public async Task<ProfileEditDto?> GetForEditAsync(string userId)
    {
        var profile = (await _profiles.FindAsync(p => p.UserId == userId)).FirstOrDefault();
        return profile is null ? null : new ProfileEditDto
        {
            Name = profile.Name,
            Bio = profile.Bio,
            DateOfBirth = profile.DateOfBirth
        };
    }

    public async Task<bool> UpdateAsync(string userId, ProfileEditDto input)
    {
        var profileId = await _profiles.GetIdByUserIdAsync(userId);
        var profile = profileId is null ? null : await _profiles.GetByIdAsync(profileId.Value);
        if (profile is null) return false;

        profile.Name = input.Name.Trim();
        profile.Bio = string.IsNullOrWhiteSpace(input.Bio) ? null : input.Bio.Trim();
        profile.DateOfBirth = input.DateOfBirth!.Value;
        _profiles.Update(profile);
        await _profiles.SaveChangesAsync();
        return true;
    }
}
