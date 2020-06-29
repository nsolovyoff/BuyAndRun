using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Category;
using BLL.Resources.Paging;
using BLL.Validators.Category;
using DAL.Entities;
using DAL.Interfaces;
using FluentValidation;

namespace BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryResource> CreateCategory(CreateCategoryResource newCategory)
        {
            var validator = new CreateCategoryResourceValidator();
            var validationResult = await validator.ValidateAsync(newCategory);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(newCategory.StartedBy.UserName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User provided in StartedBy property does not exist");

            var categoryData = _mapper.Map<CreateCategoryResource, Category>(newCategory);
            await _unitOfWork.Categories.AddAsync(categoryData);
            await _unitOfWork.CommitAsync();

            var threadModel = _mapper.Map<Category, CategoryResource>(categoryData);
            return threadModel;
        }

        public async Task DeleteCategory(int categoryToBeDeletedId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryToBeDeletedId);
            if (category == null) throw ExceptionBuilder.Create("Category with provided Id does not exist");

            _unitOfWork.Categories.Remove(category);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<CategoryResource>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);

            return result;
        }

        public async Task<PagedResultResource<CategoryResource>> GetAllPaged(int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Categories.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<CategoryResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<CategoryResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<CategoryResource> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw ExceptionBuilder.Create("Category with provided Id does not exist");

            var result = _mapper.Map<Category, CategoryResource>(category);
            return result;
        }

        public async Task<IEnumerable<CategoryResource>> GetCategoriesByAuctionId(int auctionId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var categories = auction.Categories;
            var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return result;
        }

        public async Task<PagedResultResource<CategoryResource>> GetCategoriesByAuctionIdPaged(int categoryId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var auction = await _unitOfWork.Auctions.GetByIdAsync(categoryId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var categories = auction.Categories;
            var result = new PagedResultResource<CategoryResource>
            {
                Data = _mapper.Map<IEnumerable<CategoryResource>>(categories),
                //CurrentPage = (int)page,
                //PageSize = pageSize,
                CurrentPage = 1,
                PageSize = categories.Count(),
                RowCount = categories.Count()
            };
            return result;
        }

        public async Task<IEnumerable<CategoryResource>> GetCategoriesByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var categories = user.Categories;
            var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return result;
        }

        public async Task<PagedResultResource<CategoryResource>> GetCategoriesByUserIdPaged(string userId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var lots = user.Categories.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<CategoryResource>
            {
                Data = _mapper.Map<IEnumerable<CategoryResource>>(lots),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = lots.Count()
            };
            return result;
        }

        public async Task<IEnumerable<CategoryResource>> GetCategoriesByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var categories = user.Categories;
            var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);

            return result;
        }

        public async Task<PagedResultResource<CategoryResource>> GetCategoriesByUserNamePaged(string userName, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var lots = user.Categories.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<CategoryResource>
            {
                Data = _mapper.Map<IEnumerable<CategoryResource>>(lots),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = lots.Count()
            };
            return result;
        }

        public async Task<int> GetCategoriesCountByAuctionId(int categoryId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(categoryId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var count = auction.Categories.Count;
            return count;
        }

        public async Task<int> GetCategoriesCountByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var count = user.Categories.Count;
            return count;
        }

        public async Task<int> GetCategoriesCountByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var count = user.Categories.Count;
            return count;
        }

        public async Task UpdateCategory(int categoryToBeUpdatedId, UpdateCategoryResource category)
        {
            var validator = new UpdateCategoryResourceValidator();
            var validationResult = await validator.ValidateAsync(category);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualCategory = await _unitOfWork.Categories.GetByIdAsync(categoryToBeUpdatedId);
            if (actualCategory == null) throw ExceptionBuilder.Create("Category with provided Id does not exist");

            if (category.ParentAuction != null && category.ParentAuction.Id > 0)
            {
                var auction = await _unitOfWork.Auctions.GetByIdAsync(category.ParentAuction.Id);
                actualCategory.Auction = (auction != null) ? auction : actualCategory.Auction;
            }
            actualCategory.Title = (string.IsNullOrEmpty(category.Title)) ? category.Title : actualCategory.Title;

            await _unitOfWork.CommitAsync();
        }
    }
}
