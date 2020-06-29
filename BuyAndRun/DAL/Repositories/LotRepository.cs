using DAL.Contexts;
using DAL.Entities;
using DAL.Interfaces.Repositories;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class LotRepository : BaseRepository<Lot>, ILotRepository
    {
        public LotRepository(AuctionDbContext context) : base(context) { }
    }
}
