using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class AuctionToModeratorConfiguration : IEntityTypeConfiguration<AuctionToModerator>
    {
        public void Configure(EntityTypeBuilder<AuctionToModerator> builder)
        {
            builder
                .HasKey(r => new { r.UserId, AuctionId = r.AuctionId });

            builder
                .HasOne(r => r.User)
                .WithMany(u => u.Auctions)
                .HasForeignKey(r => r.UserId);

            builder
                .HasOne(r => r.Auction)
                .WithMany(f => f.Moderators)
                .HasForeignKey(r => r.AuctionId);
        }
    }
}
