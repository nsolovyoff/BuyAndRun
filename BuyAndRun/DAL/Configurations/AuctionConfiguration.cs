using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder
                .Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(64);
            
            builder
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder
                .Property(f => f.IsActive)
                .ValueGeneratedNever();
        }
    }
}
