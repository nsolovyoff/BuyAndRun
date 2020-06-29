using System;
using System.Threading.Tasks;
using DAL.Contexts;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using DAL.Repositories;
using DAL.Repositories.Identity;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuctionDbContext _context;

        private LotRepository _lotRepository;
        private AuctionRepository _auctionRepository;
        private CategoryRepository _categoryRepository;
        private IUserRepository _userRepository;
        private AuctionToModeratorRepository _auctionToModeratorRepository;

        public UnitOfWork(AuctionDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAuctionRepository Auctions => _auctionRepository = _auctionRepository ?? new AuctionRepository(_context);

        public ILotRepository Lots => _lotRepository = _lotRepository ?? new LotRepository(_context);

        public ICategoryRepository Categories => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IAuctionToModeratorRepository AuctionToModerators => _auctionToModeratorRepository = _auctionToModeratorRepository ?? new AuctionToModeratorRepository(_context);

        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
