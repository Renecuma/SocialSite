using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Web.Extensions;

namespace SocialSite.Web.Areas.Admin.Controllers;

// Uživatele a role smí spravovat pouze Admin
[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class UsersController : Controller
{
    private readonly IUserAdminService _userAdminService;

    public UsersController(IUserAdminService userAdminService)
    {
        _userAdminService = userAdminService;
    }

    public async Task<IActionResult> Index()
        => View(await _userAdminService.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var input = await _userAdminService.GetForEditAsync(id);
        if (input is null) return NotFound();

        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditDto input)
    {
        if (!ModelState.IsValid)
            return View(input);

        var result = await _userAdminService.UpdateAsync(input);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);
            return View(input);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _userAdminService.DeleteAsync(id, User.GetUserId()!);
        if (!result.Succeeded)
            TempData["Error"] = string.Join(" ", result.Errors);

        return RedirectToAction(nameof(Index));
    }
}
