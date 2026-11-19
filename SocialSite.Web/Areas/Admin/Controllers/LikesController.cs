using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Common;
using SocialSite.Application.Interfaces;

namespace SocialSite.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class LikesController : Controller
{
    private readonly ILikeService _likeService;

    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    public async Task<IActionResult> Index()
        => View(await _likeService.GetAllAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _likeService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
