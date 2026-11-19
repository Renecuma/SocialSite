using SocialSite.Application.DTOs;

namespace SocialSite.Web.Models;

public class PostDetailViewModel
{
    public PostDto Post { get; set; } = null!;
    public CommentInputDto NewComment { get; set; } = new();
}
