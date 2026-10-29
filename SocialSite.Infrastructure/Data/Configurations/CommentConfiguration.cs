using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Core.Entities;

namespace SocialSite.Infrastructure.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> b)
    {
        b.Property(c => c.Text).IsRequired().HasMaxLength(1000);

        b.HasOne(c => c.Post)
         .WithMany(p => p.Comments)
         .HasForeignKey(c => c.PostId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(c => c.UserProfile)
         .WithMany(u => u.Comments)
         .HasForeignKey(c => c.UserProfileId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
