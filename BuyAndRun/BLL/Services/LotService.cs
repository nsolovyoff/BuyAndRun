using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Lot;
using BLL.Resources.Paging;
using BLL.Validators.Lot;
using DAL.Entities;
using DAL.Interfaces;
using FluentValidation;

namespace BLL.Services
{
    public class LotService : ILotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LotResource> CreateLot(CreateLotResource newLot)
        {
            var lotData = _mapper.Map<CreateLotResource, Lot>(newLot);

            await _unitOfWork.Lots.AddAsync(lotData);
            await _unitOfWork.CommitAsync();

            var lotModel = _mapper.Map<Lot, LotResource>(lotData);
            return lotModel;
        }


        public async Task DeleteLot(int lotToBeDeletedId)
        {
            var lot = await _unitOfWork.Lots.GetByIdAsync(lotToBeDeletedId);
            if (lot == null) throw ExceptionBuilder.Create("Lot with provided Id does not exist");

            _unitOfWork.Lots.Remove(lot);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<LotResource>> GetAll()
        {
            var lots = await _unitOfWork.Lots.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Lot>, IEnumerable<LotResource>>(lots);

            return result;
        }

        public async Task<PagedResultResource<LotResource>> GetAllPaged(int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Lots.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<LotResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<LotResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<LotResource> GetLotById(int id)
        {
            var lot = await _unitOfWork.Lots.GetByIdAsync(id);
            if (lot == null) throw ExceptionBuilder.Create("Lot with provided Id does not exist");

            var result = _mapper.Map<Lot, LotResource>(lot);
            return result;
        }

        public async Task<IEnumerable<LotResource>> GetLotsByAuctionId(int auctionId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var lots = auction.Categories.SelectMany(t => t.Lots);
            var result = _mapper.Map<IEnumerable<Lot>, IEnumerable<LotResource>>(lots);
            return result;
        }

        public async Task<PagedResultResource<LotResource>> GetLotsByAuctionIdPaged(int auctionId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");
            
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var lots = auction.Categories.SelectMany(t => t.Lots).Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<LotResource>
            {
                Data = _mapper.Map<IEnumerable<LotResource>>(lots),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = lots.Count()
            };
            return result;
        }

        public async Task<IEnumerable<LotResource>> GetLotsByCategoryId(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null) throw ExceptionBuilder.Create("Category with provided Id does not exist");

            var lots = category.Lots;
            var result = _mapper.Map<IEnumerable<LotResource>>(lots);
            return result;
        }

        public async Task<PagedResultResource<LotResource>> GetLotsByCategoryIdPaged(int categoryId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null) throw ExceptionBuilder.Create("Category with provided Id does not exist");

            var lots = category.Lots.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<LotResource>
            {
                Data = _mapper.Map<IEnumerable<LotResource>>(lots),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = lots.Count()
            };
            return result;
        }

        public async Task<IEnumerable<LotResource>> GetLotsByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var lots = user.Lots;
            var result = _mapper.Map<IEnumerable<Lot>, IEnumerable<LotResource>>(lots);
            return result;
        }

        public async Task<PagedResultResource<LotResource>> GetLotsByUserIdPaged(string userId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var lots = user.Lots.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<LotResource>
            {
                Data = _mapper.Map<IEnumerable<LotResource>>(lots),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = lots.Count()
            };
            return result;
        }

        public async Task<IEnumerable<LotResource>> GetLotsByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("Lot with provided UserName does not exist");

            var lots = user.Lots;
            var result = _mapper.Map<IEnumerable<Lot>, IEnumerable<LotResource>>(lots);
            return result;
        }

        public async Task<PagedResultResource<LotResource>> GetLotsByUserNamePaged(string userName, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var lots = user.Lots.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<LotResource>
            {
                Data = _mapper.Map<IEnumerable<LotResource>>(lots),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = lots.Count()
            };
            return result;
        }

        public async Task<int> GetLotsCountByAuctionId(int auctionId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            int count = auction.Categories.SelectMany(t => t.Lots).Count();
            return count;
        }

        public async Task<int> GetLotsCountByCategoryId(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null) throw ExceptionBuilder.Create("Category with provided Id does not exist");

            var count = category.Lots.Count;
            return count;
        }

        public async Task<int> GetLotsCountByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            int count = user.Lots.Count;
            return count;
        }

        public async Task<int> GetLotsCountByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            int count = user.Lots.Count;
            return count;
        }

        public async Task UpdateLot(int lotToBeUpdatedId, UpdateLotResource lot)
        {
            var validator = new UpdateLotResourceValidator();
            var validationResult = await validator.ValidateAsync(lot);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualLot = await _unitOfWork.Lots.GetByIdAsync(lotToBeUpdatedId);
            if (actualLot == null) throw ExceptionBuilder.Create("Lot with provided Id does not exist");
            
            actualLot.Title = (!string.IsNullOrEmpty(lot.Title)) ? lot.Title : actualLot.Title;
            actualLot.Description = (!string.IsNullOrEmpty(lot.Description)) ? lot.Description : actualLot.Description;
            actualLot.BuyNowPrice = (lot.BuyNowPrice != 0) ? lot.BuyNowPrice : actualLot.BuyNowPrice;
            actualLot.Bid = (lot.Bid != 0) ? lot.Bid : actualLot.Bid;
            actualLot.BidUser = (!string.IsNullOrEmpty(lot.BidUser)) ? lot.BidUser : actualLot.BidUser;
            actualLot.Expiring = (lot.Expiring != DateTime.MinValue) ? lot.Expiring : actualLot.Expiring;

            await _unitOfWork.CommitAsync();
        }


        public async Task MakeBid(int lotToBeUpdatedId, LotResource lot)
        {
            var validator = new MakeBidResourceValidator();
            var validationResult = await validator.ValidateAsync(lot);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualLot = await _unitOfWork.Lots.GetByIdAsync(lotToBeUpdatedId);
            if (actualLot == null) throw ExceptionBuilder.Create("Lot with provided Id does not exist");
            
            actualLot.Bid = (lot.Bid != 0) ? lot.Bid : actualLot.Bid;
            actualLot.BidUser = (!string.IsNullOrEmpty(lot.BidUser)) ? lot.BidUser : actualLot.BidUser;

            await _unitOfWork.CommitAsync();
        }
        
        
        public async Task BuyNow(int lotToBeUpdatedId, string bidUser)
        {
            var actualLot = await _unitOfWork.Lots.GetByIdAsync(lotToBeUpdatedId);
            if (actualLot == null) throw ExceptionBuilder.Create("Lot with provided Id does not exist");
            
            actualLot.Bid = actualLot.BuyNowPrice;
            actualLot.Expiring = DateTime.Now.AddDays(-1);
            actualLot.BidUser = bidUser;

            await _unitOfWork.CommitAsync();
        }
    }
}
