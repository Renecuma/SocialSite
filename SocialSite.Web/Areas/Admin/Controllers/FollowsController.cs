using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Common;
using SocialSite.Application.Interfaces;

namespace SocialSite.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class FollowsController : Controller
{
    private readonly IFollowService _followService;

    public FollowsController(IFollowService followService)
    {
        _followService = followService;
    }

    public async Task<IActionResult> Index()
        => View(await _followService.GetAllAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _followService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
