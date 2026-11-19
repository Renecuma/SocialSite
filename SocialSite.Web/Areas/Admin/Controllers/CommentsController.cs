using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Common;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;

namespace SocialSite.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.AdminOrManager)]
public class CommentsController : Controller
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
        => View(await _commentService.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var input = await _commentService.GetForEditAsync(id);
        if (input is null) return NotFound();

        ViewData["CommentId"] = id;
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CommentInputDto input)
    {
        if (!ModelState.IsValid)
        {
            ViewData["CommentId"] = id;
            return View(input);
        }

        if (!await _commentService.UpdateAsync(id, input))
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _commentService.DeleteAsync(id, ownerUserId: null);
        return RedirectToAction(nameof(Index));
    }
}
