using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Resources.Auction;
using BLL.Resources.Paging;

namespace BLL.Interfaces
{
    public interface IAuctionService
    {
        Task<IEnumerable<AuctionResource>> GetAll();
        Task<PagedResultResource<AuctionResource>> GetAllPaged(int? page, int pageSize);
        Task<AuctionResource> GetAuctionById(int id);

        Task<AuctionResource> CreateAuction(CreateAuctionResource auction);
        Task UpdateAuction(int auctionToBeUpdatedId, UpdateAuctionResource auction);

        Task DeleteAuction(int auctionToBeDeletedId);
        Task AddModeratorToAuction(AddModeratorToAuctionResource model);
    }
}
