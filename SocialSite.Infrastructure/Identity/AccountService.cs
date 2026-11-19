using Microsoft.AspNetCore.Identity;
using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Core.Entities;
using SocialSite.Core.Interfaces;

namespace SocialSite.Infrastructure.Identity;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IRepository<UserProfile> _profiles;

    public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IRepository<UserProfile> profiles)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _profiles = profiles;
    }

    public async Task<OperationResult> RegisterAsync(RegisterDto input)
    {
        var user = new IdentityUser { UserName = input.Email, Email = input.Email };

        var result = await _userManager.CreateAsync(user, input.Password);
        if (!result.Succeeded)
            return OperationResult.Failure(result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, Roles.User);

        await _profiles.AddAsync(new UserProfile
        {
            UserId = user.Id,
            Name = input.Name.Trim(),
            DateOfBirth = input.DateOfBirth!.Value
        });
        await _profiles.SaveChangesAsync();

        await _signInManager.SignInAsync(user, isPersistent: false);
        return OperationResult.Success();
    }

    public async Task<bool> LoginAsync(LoginDto input)
    {
        var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, lockoutOnFailure: false);
        return result.Succeeded;
    }

    public Task LogoutAsync() => _signInManager.SignOutAsync();
}
