using System;
using System.Threading.Tasks;
using DAL.Interfaces.Repositories;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuctionRepository Auctions { get; }
        ILotRepository Lots { get; }
        ICategoryRepository Categories { get; }
        IAuctionToModeratorRepository AuctionToModerators { get; }
        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}
