using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;

namespace SocialSite.Web.Areas.Admin.Controllers;

// Obsah moderuje Admin i Manager
[Area("Admin")]
[Authorize(Roles = Roles.AdminOrManager)]
public class PostsController : Controller
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index()
        => View(await _postService.GetFeedAsync(null));

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var input = await _postService.GetForEditAsync(id, ownerUserId: null);
        if (input is null) return NotFound();

        ViewData["PostId"] = id;
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PostInputDto input)
    {
        if (!ModelState.IsValid)
        {
            ViewData["PostId"] = id;
            return View(input);
        }

        if (!await _postService.UpdateAsync(id, input, ownerUserId: null))
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _postService.DeleteAsync(id, ownerUserId: null);
        return RedirectToAction(nameof(Index));
    }
}
