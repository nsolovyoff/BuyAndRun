using System;
using System.Threading.Tasks;
using DAL.Contexts;
using DAL.Entities.Identity;
using DAL.Interfaces.Repositories;
using DAL.Repositories.Base;

namespace DAL.Repositories.Identity
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly Microsoft.EntityFrameworkCore.DbContext _context;

        public UserRepository(AuctionDbContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ValueTask<User> GetByIdAsync(string id)
        {
            return _context.Set<User>().FindAsync(id);
        }
    }
}
