using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Application.Common;

namespace SocialSite.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.AdminOrManager)]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
