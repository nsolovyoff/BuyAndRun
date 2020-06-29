using AutoMapper;
using BLL.Resources.Auction;
using DAL.Entities;

namespace BLL.Mapping.ValueResolvers.AuctionResolvers
{
    public class LotsCountAuctionResolver : IValueResolver<Auction, AuctionResource, int>
    {
        public LotsCountAuctionResolver() { }

        public int Resolve(Auction source, AuctionResource destination, int destMember, ResolutionContext context)
        {
            int count = 0;
            foreach (var category in source.Categories)
            {
                count += category.Lots.Count;
            }

            return count;
        }
    }
}
