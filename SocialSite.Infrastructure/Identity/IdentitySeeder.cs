using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialSite.Application.Common;
using SocialSite.Core.Entities;
using SocialSite.Infrastructure.Data;

namespace SocialSite.Infrastructure.Identity;

// Při startu aplikuje migrace, vytvoří role a výchozího admina (údaje z appsettings.json, sekce "Admin")
public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var config = provider.GetRequiredService<IConfiguration>();
        var email = config["Admin:Email"];
        var password = config["Admin:Password"];
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return;

        var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var admin = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(admin, password);
        if (!result.Succeeded)
            return;

        await userManager.AddToRoleAsync(admin, Roles.Admin);
        context.UserProfiles.Add(new UserProfile
        {
            UserId = admin.Id,
            Name = "Administrátor",
            DateOfBirth = new DateOnly(2000, 1, 1)
        });
        await context.SaveChangesAsync();
    }
}
