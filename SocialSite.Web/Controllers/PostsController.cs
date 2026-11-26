using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.DTOs;
using SocialSite.Application.Interfaces;
using SocialSite.Web.Extensions;
using SocialSite.Web.Models;

namespace SocialSite.Web.Controllers;

[Authorize]
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly ILikeService _likeService;

    public PostsController(IPostService postService, ICommentService commentService, ILikeService likeService)
    {
        _postService = postService;
        _commentService = commentService;
        _likeService = likeService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var post = await _postService.GetDetailAsync(id, User.GetUserId());
        if (post is null) return NotFound();

        return View(new PostDetailViewModel { Post = post, NewComment = new CommentInputDto { PostId = id } });
    }

    [HttpGet]
    public IActionResult Create() => View(new PostInputDto());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostInputDto input, IFormFile? image)
    {
        var upload = image.ToImageUpload(ModelState);
        if (!ModelState.IsValid)
            return View(input);

        var id = await _postService.CreateAsync(input, upload, User.GetUserId()!);
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var input = await _postService.GetForEditAsync(id, User.GetUserId());
        if (input is null) return NotFound();

        ViewData["PostId"] = id;
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PostInputDto input, IFormFile? image)
    {
        var upload = image.ToImageUpload(ModelState);
        if (!ModelState.IsValid)
        {
            ViewData["PostId"] = id;
            return View(input);
        }

        if (!await _postService.UpdateAsync(id, input, upload, User.GetUserId()))
            return NotFound();

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _postService.DeleteAsync(id, User.GetUserId()))
            return NotFound();

        return RedirectToAction("Index", "Profile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Like(int id, string? returnUrl = null)
    {
        await _likeService.ToggleAsync(id, User.GetUserId()!);
        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Comment([Bind(Prefix = nameof(PostDetailViewModel.NewComment))] CommentInputDto input,
        IFormFile? image)
    {
        var upload = image.ToImageUpload(ModelState);
        if (!ModelState.IsValid)
        {
            var post = await _postService.GetDetailAsync(input.PostId, User.GetUserId());
            if (post is null) return NotFound();
            return View(nameof(Details), new PostDetailViewModel { Post = post, NewComment = input });
        }

        if (!await _commentService.AddAsync(input, upload, User.GetUserId()!))
            return NotFound();

        return RedirectToAction(nameof(Details), new { id = input.PostId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var postId = await _commentService.DeleteAsync(id, User.GetUserId());
        if (postId is null) return NotFound();

        return RedirectToAction(nameof(Details), new { id = postId });
    }
}
