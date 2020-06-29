using DAL.Contexts;
using DAL.Entities;
using DAL.Interfaces.Repositories;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AuctionDbContext context) : base(context) { }
    }
}
