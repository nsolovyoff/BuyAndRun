using DAL.Contexts;
using DAL.Entities;
using DAL.Interfaces.Repositories;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
    {
        public AuctionRepository(AuctionDbContext context) : base(context) { }
    }
}
