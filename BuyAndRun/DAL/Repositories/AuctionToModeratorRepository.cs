using DAL.Contexts;
using DAL.Entities;
using DAL.Interfaces.Repositories;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class AuctionToModeratorRepository : BaseRepository<AuctionToModerator>, IAuctionToModeratorRepository
    {
        public AuctionToModeratorRepository(AuctionDbContext context) : base(context) { }
    }
}
