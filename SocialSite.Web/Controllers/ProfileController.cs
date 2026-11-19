using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Web.Extensions;

namespace SocialSite.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly IFollowService _followService;

    public ProfileController(IProfileService profileService, IFollowService followService)
    {
        _profileService = profileService;
        _followService = followService;
    }

    // /Profile = můj profil, /Profile/Index/5 = cizí profil
    public async Task<IActionResult> Index(int? id)
    {
        var profile = await _profileService.GetAsync(id, User.GetUserId());
        if (profile is null) return NotFound();

        return View(profile);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var input = await _profileService.GetForEditAsync(User.GetUserId()!);
        if (input is null) return NotFound();

        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProfileEditDto input)
    {
        if (!ModelState.IsValid)
            return View(input);

        if (!await _profileService.UpdateAsync(User.GetUserId()!, input))
            return NotFound();

        TempData["Message"] = "Změny byly uloženy.";
        return RedirectToAction(nameof(Edit));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Follow(int id)
    {
        await _followService.ToggleAsync(id, User.GetUserId()!);
        return RedirectToAction(nameof(Index), new { id });
    }
}
