using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialSite.Core.Entities;

namespace SocialSite.Infrastructure.Data.Configurations;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> b)
    {
        b.HasOne(f => f.Sender)
         .WithMany(u => u.Following)
         .HasForeignKey(f => f.SenderId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(f => f.Receiver)
         .WithMany(u => u.Followers)
         .HasForeignKey(f => f.ReceiverId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(f => new { f.SenderId, f.ReceiverId }).IsUnique();

        b.ToTable(t => t.HasCheckConstraint("CK_Follow_NotSelf", "[SenderId] <> [ReceiverId]"));
    }
}
