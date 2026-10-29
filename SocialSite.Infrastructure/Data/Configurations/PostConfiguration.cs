using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Core.Entities;

namespace SocialSite.Infrastructure.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> b)
    {
        b.Property(p => p.Content).IsRequired().HasMaxLength(2000);
        b.Property(p => p.ImagePath).HasMaxLength(260);

        b.HasOne(p => p.UserProfile)
         .WithMany(u => u.Posts)
         .HasForeignKey(p => p.UserProfileId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(p => p.CreatedAt);
    }
}
