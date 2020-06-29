using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Resources.Identity.User;
using BLL.Resources.Paging;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResource>> GetAll();
        Task<PagedResultResource<UserResource>> GetAllPaged(int? page, int pageSize);

        Task<UserResource> GetUserById(string id);
        Task<UserResource> GetUserByUserName(string userName);

        Task<UserResource> CreateUser(CreateUserResource newUser);
        Task UpdateUser(string userName, UpdateUserResource user);
        Task DeleteUser(string userName);
        Task AddUserToRole(string userName, string role);

        Task<IEnumerable<UserResource>> GetUsersAsAuctionModerators(int auctionId);
        Task<PagedResultResource<UserResource>> GetUsersAsAuctionModeratorsPaged(int auctionId, int? page, int pageSize);

    }
}
