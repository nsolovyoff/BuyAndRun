using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class LotConfiguration : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            // builder
            //     .HasOne(p => p.User)
            //     .WithMany(u => u.Lots)
            //     .HasForeignKey(p => p.UserId)
            //     .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .Property(p => p.User)
                .IsRequired();

            builder
                .Property(p => p.Title)
                .IsRequired();
            
            builder
                .Property(p => p.Bid)
                .IsRequired();
            
            builder
                .Property(p => p.ImageUrl)
                .IsRequired();

            builder
                .Property(p => p.BidUser)
                .IsRequired();
            
            builder
                .Property(p => p.Expiring)
                .IsRequired();

            builder
                .Property(p => p.Description)
                .IsRequired();

            builder
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
