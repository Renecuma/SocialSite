using Microsoft.AspNetCore.Identity;
using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Infrastructure.Identity;

// Správa uživatelů adminem – záměrně bez možnosti měnit heslo (PasswordHash)
public class UserAdminService : IUserAdminService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IRepository<UserProfile> _profiles;
    private readonly IRepository<Comment> _comments;
    private readonly IRepository<Like> _likes;
    private readonly IRepository<Follow> _follows;

    public UserAdminService(UserManager<IdentityUser> userManager, IRepository<UserProfile> profiles,
        IRepository<Comment> comments, IRepository<Like> likes, IRepository<Follow> follows)
    {
        _userManager = userManager;
        _profiles = profiles;
        _comments = comments;
        _likes = likes;
        _follows = follows;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync()
    {
        var profiles = (await _profiles.GetAllAsync()).ToDictionary(p => p.UserId);
        var result = new List<UserDto>();

        foreach (var user in _userManager.Users.OrderBy(u => u.Email).ToList())
        {
            profiles.TryGetValue(user.Id, out var profile);
            result.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Name = profile?.Name ?? string.Empty,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty,
                ProfileId = profile?.Id
            });
        }

        return result;
    }

    public async Task<UserEditDto?> GetForEditAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return null;

        var profile = (await _profiles.FindAsync(p => p.UserId == id)).FirstOrDefault();
        return new UserEditDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            Name = profile?.Name ?? string.Empty,
            Bio = profile?.Bio,
            DateOfBirth = profile?.DateOfBirth,
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? Roles.User
        };
    }

    public async Task<OperationResult> UpdateAsync(UserEditDto input)
    {
        if (!Roles.All.Contains(input.Role))
            return OperationResult.Failure("Neplatná role.");

        var user = await _userManager.FindByIdAsync(input.Id);
        if (user is null)
            return OperationResult.Failure("Uživatel neexistuje.");

        user.Email = input.Email;
        user.UserName = input.Email;
        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
            return OperationResult.Failure(update.Errors.Select(e => e.Description));

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (!currentRoles.SequenceEqual([input.Role]))
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, input.Role);
        }

        var profileId = (await _profiles.FindAsync(p => p.UserId == user.Id)).FirstOrDefault()?.Id;
        var profile = profileId is null ? new UserProfile { UserId = user.Id } : await _profiles.GetByIdAsync(profileId.Value);
        profile!.Name = input.Name.Trim();
        profile.Bio = string.IsNullOrWhiteSpace(input.Bio) ? null : input.Bio.Trim();
        profile.DateOfBirth = input.DateOfBirth!.Value;
        if (profileId is null)
            await _profiles.AddAsync(profile);
        await _profiles.SaveChangesAsync();

        return OperationResult.Success();
    }

    public async Task<OperationResult> DeleteAsync(string id, string currentUserId)
    {
        if (id == currentUserId)
            return OperationResult.Failure("Nemůžete smazat sám sebe.");

        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
            return OperationResult.Failure("Uživatel neexistuje.");

        // Komentáře, lajky a sledování mají Restrict, proto je smažeme ručně.
        // Profil a jeho příspěvky se smažou kaskádově spolu s účtem.
        var profile = (await _profiles.FindAsync(p => p.UserId == id)).FirstOrDefault();
        if (profile is not null)
        {
            _comments.RemoveRange(await _comments.FindAsync(c => c.UserProfileId == profile.Id));
            _likes.RemoveRange(await _likes.FindAsync(l => l.UserProfileId == profile.Id));
            _follows.RemoveRange(await _follows.FindAsync(f => f.SenderId == profile.Id || f.ReceiverId == profile.Id));
            await _profiles.SaveChangesAsync();
        }

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded
            ? OperationResult.Success()
            : OperationResult.Failure(result.Errors.Select(e => e.Description));
    }
}
