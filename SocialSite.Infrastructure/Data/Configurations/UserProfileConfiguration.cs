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
    }
}
