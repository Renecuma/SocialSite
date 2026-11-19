using Microsoft.Extensions.DependencyInjection;
using SocialSite.Application.Interfaces;
using SocialSite.Application.Services;

namespace SocialSite.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<IFollowService, FollowService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
