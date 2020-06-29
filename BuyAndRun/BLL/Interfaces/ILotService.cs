using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Resources.Lot;
using BLL.Resources.Paging;

namespace BLL.Interfaces
{
    public interface ILotService
    {
        Task<IEnumerable<LotResource>> GetAll();
        Task<PagedResultResource<LotResource>> GetAllPaged(int? page, int pageSize);

        Task<LotResource> GetLotById(int id);

        Task<IEnumerable<LotResource>> GetLotsByCategoryId(int categoryId);
        Task<PagedResultResource<LotResource>> GetLotsByCategoryIdPaged(int categoryId, int? page, int pageSize);

        Task<int> GetLotsCountByCategoryId(int categoryId);

        Task<IEnumerable<LotResource>> GetLotsByAuctionId(int auctionId);
        Task<PagedResultResource<LotResource>> GetLotsByAuctionIdPaged(int auctionId, int? page, int pageSize);

        Task<int> GetLotsCountByAuctionId(int auctionId);

        Task<IEnumerable<LotResource>> GetLotsByUserId(string userId);
        Task<PagedResultResource<LotResource>> GetLotsByUserIdPaged(string userId, int? page, int pageSize);

        Task<int> GetLotsCountByUserId(string userId);

        Task<IEnumerable<LotResource>> GetLotsByUserName(string userName);
        Task<PagedResultResource<LotResource>> GetLotsByUserNamePaged(string userName, int? page, int pageSize);
        Task<int> GetLotsCountByUserName(string userName);

        Task<LotResource> CreateLot(CreateLotResource newLot);
        Task UpdateLot(int lotToBeUpdatedId, UpdateLotResource lot);
        Task MakeBid(int lotToBeUpdatedId, LotResource lot);
        Task BuyNow(int lotToBeUpdatedId, string bidUser);

        Task DeleteLot(int lotToBeDeletedId);
    }
}
