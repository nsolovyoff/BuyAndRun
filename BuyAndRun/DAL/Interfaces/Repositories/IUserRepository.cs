using DAL.Entities.Identity;
using DAL.Interfaces.Repositories.Base;

namespace DAL.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User, string>
    {
        
    }
}
