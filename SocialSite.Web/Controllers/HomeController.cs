using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Interfaces;
using SocialSite.Web.Extensions;

namespace SocialSite.Web.Controllers;

public class HomeController : Controller
{
    private readonly IPostService _postService;

    public HomeController(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index()
    {
        var feed = await _postService.GetFeedAsync(User.GetUserId());
        return View(feed);
    }

    public IActionResult Error() => View();
}
