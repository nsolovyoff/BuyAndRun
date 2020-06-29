using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using BLL.Resources.Auction;
using BLL.Resources.Paging;
using BLL.Validators.Auction;
using DAL.Entities;
using DAL.Interfaces;
using FluentValidation;

namespace BLL.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuctionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddModeratorToAuction(AddModeratorToAuctionResource resource)
        {
            // todo: add validator, add api endpoint for method
            //var validator = new AddModeratorToauctionResourceValidator();
            //var validationResult = await validator.ValidateAsync(resource);
            //if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var data = _mapper.Map<AddModeratorToAuctionResource, AuctionToModerator>(resource);
            await _unitOfWork.AuctionToModerators.AddAsync(data);
            await _unitOfWork.CommitAsync();
        }

        public async Task<AuctionResource> CreateAuction(CreateAuctionResource auction)
        {
            var validator = new CreateAuctionResourceValidator();
            var validationResult = await validator.ValidateAsync(auction);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var auctionData = _mapper.Map<CreateAuctionResource, Auction>(auction);

            await _unitOfWork.Auctions.AddAsync(auctionData);
            await _unitOfWork.CommitAsync();

            var auctionModel = _mapper.Map<Auction, AuctionResource>(auctionData);

            return auctionModel;
        }

        public async Task DeleteAuction(int auctionToBeDeletedId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionToBeDeletedId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            _unitOfWork.Auctions.Remove(auction);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<AuctionResource>> GetAll()
        {
            var auctions = await _unitOfWork.Auctions.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Auction>, IEnumerable<AuctionResource>>(auctions);

            return result;
        }

        public async Task<PagedResultResource<AuctionResource>> GetAllPaged(int? page, int pageSize = 5)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Auctions.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<AuctionResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<AuctionResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<AuctionResource> GetAuctionById(int id)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(id);
            if (auction == null) throw ExceptionBuilder.Create("auction with provided Id does not exist");

            var result = _mapper.Map<Auction, AuctionResource>(auction);
            return result;
        }

        public async Task UpdateAuction(int auctionToBeUpdatedId, UpdateAuctionResource auction)
        {
            var validator = new UpdateAuctionResourceValidator();
            var validationResult = await validator.ValidateAsync(auction);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualAuction = await _unitOfWork.Auctions.GetByIdAsync(auctionToBeUpdatedId);
            if (actualAuction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            actualAuction.Name = (!string.IsNullOrEmpty(auction.Name)) ? auction.Name : actualAuction.Name;
            actualAuction.Description = (!string.IsNullOrEmpty(auction.Description)) ? auction.Description : actualAuction.Description;
            actualAuction.IsActive = (auction.IsActive.HasValue) ? (bool)auction.IsActive : actualAuction.IsActive;

            await _unitOfWork.CommitAsync();
        }
    }
}
