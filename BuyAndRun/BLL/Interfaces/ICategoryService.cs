using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Resources.Category;
using BLL.Resources.Paging;

namespace BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResource>> GetAll();
        Task<PagedResultResource<CategoryResource>> GetAllPaged(int? page, int pageSize);

        Task<CategoryResource> GetCategoryById(int id);

        Task<IEnumerable<CategoryResource>> GetCategoriesByAuctionId(int auctionId);
        Task<PagedResultResource<CategoryResource>> GetCategoriesByAuctionIdPaged(int categoryId, int? page, int pageSize);

        Task<int> GetCategoriesCountByAuctionId(int categoryId);

        Task<IEnumerable<CategoryResource>> GetCategoriesByUserId(string userId);
        Task<PagedResultResource<CategoryResource>> GetCategoriesByUserIdPaged(string userId, int? page, int pageSize);
        Task<int> GetCategoriesCountByUserId(string userId);

        Task<IEnumerable<CategoryResource>> GetCategoriesByUserName(string userName);
        Task<PagedResultResource<CategoryResource>> GetCategoriesByUserNamePaged(string userName, int? page, int pageSize);
        Task<int> GetCategoriesCountByUserName(string userName);

        Task<CategoryResource> CreateCategory(CreateCategoryResource newCategory);
        Task UpdateCategory(int categoryToBeUpdatedId, UpdateCategoryResource category);
        Task DeleteCategory(int categoryToBeDeletedId);
    }
}
