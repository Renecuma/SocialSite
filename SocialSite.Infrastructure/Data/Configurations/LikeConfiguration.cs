using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Core.Entities;

namespace SocialSite.Infrastructure.Data.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> b)
    {
        b.HasOne(l => l.Post)
         .WithMany(p => p.Likes)
         .HasForeignKey(l => l.PostId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(l => l.UserProfile)
         .WithMany(u => u.Likes)
         .HasForeignKey(l => l.UserProfileId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(l => new { l.PostId, l.UserProfileId }).IsUnique();
    }
}
