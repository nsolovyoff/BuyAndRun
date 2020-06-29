using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Identity;
using BLL.Interfaces;
using BLL.Resources.Identity.User;
using BLL.Resources.Paging;
using BLL.Validators.Identity.User;
using DAL.Entities;
using DAL.Entities.Identity;
using DAL.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public class UserSerivce : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuctionUserManager _userManager; // for user creation, roles ..

        public UserSerivce(IMapper mapper, IUnitOfWork unitOfWork, AuctionUserManager userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task AddUserToRole(string userName, string role)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) throw ExceptionBuilder.Create("Eror when adding roles to user - " + result.ToString(), HttpStatusCode.BadRequest);
        }

        public async Task<UserResource> CreateUser(CreateUserResource newUser)
        {
            var validator = new CreateUserResourceValidator();
            var validationResult = await validator.ValidateAsync(newUser);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var user = _mapper.Map<CreateUserResource, User>(newUser);
            IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded) throw ExceptionBuilder.Create("Eror when creating user - " + result.ToString(), HttpStatusCode.BadRequest);

            var userModel = _mapper.Map<UserResource>(user);
            return userModel;
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<UserResource>> GetAll()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var result = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return result;
        }

        public async Task<PagedResultResource<UserResource>> GetAllPaged(int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Users.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<UserResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<UserResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<UserResource> GetUserById(string id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var result = _mapper.Map<User, UserResource>(user);
            return result;
        }

        public async Task<UserResource> GetUserByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var result = _mapper.Map<User, UserResource>(user);
            return result;
        }

        public async Task<IEnumerable<UserResource>> GetUsersAsAuctionModerators(int auctionId)
        {
            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var auctionMods = auction.Moderators;
            var result = _mapper.Map<IEnumerable<AuctionToModerator>, IEnumerable<UserResource>>(auctionMods);
            return result;
        }

        public async Task<PagedResultResource<UserResource>> GetUsersAsAuctionModeratorsPaged(int auctionId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var auction = await _unitOfWork.Auctions.GetByIdAsync(auctionId);
            if (auction == null) throw ExceptionBuilder.Create("Auction with provided Id does not exist");

            var users = auction.Moderators.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<UserResource>
            {
                Data = _mapper.Map<IEnumerable<UserResource>>(users),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = users.Count()
            };
            return result;
        }

        public async Task UpdateUser(string userName, UpdateUserResource user)
        {
            var validator = new UpdateUserResourceValidator();
            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (actualUser == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            actualUser.Name = (string.IsNullOrEmpty(user.Name)) ? user.Name : actualUser.Name;
            actualUser.Email = (string.IsNullOrEmpty(user.Email)) ? user.Email : actualUser.Email;

            await _unitOfWork.CommitAsync();
        }
    }
}
