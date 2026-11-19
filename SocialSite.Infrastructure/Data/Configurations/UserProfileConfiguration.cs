using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Core.Entities;

namespace SocialSite.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> b)
    {
        b.Property(u => u.Name).IsRequired().HasMaxLength(100);
        b.Property(u => u.Bio).HasMaxLength(500);
        b.Property(u => u.ProfilePicturePath).HasMaxLength(260);

        // 1:1 vazba profilu na Identity účet
        b.Property(u => u.UserId).IsRequired();
        b.HasIndex(u => u.UserId).IsUnique();
        b.HasOne<IdentityUser>()
         .WithOne()
         .HasForeignKey<UserProfile>(u => u.UserId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
