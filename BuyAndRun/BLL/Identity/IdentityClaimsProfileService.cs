﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Entities.Identity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace BLL.Identity
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly AuctionUserManager _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;

        public IdentityClaimsProfileService(AuctionUserManager userManager, IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims
                .Where(claim => context.RequestedClaimTypes.Contains(claim.Type))
                .ToList();

            #region UncommentForUserClaims
            //var userClaims = await _userManager.GetClaimsAsync(user);
            //foreach (var claim in userClaims)
            //{
            //    claims.Add(claim);
            //} 
            #endregion

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var userAuctions = user.Auctions;
            foreach (var auction in userAuctions)
            {
                claims.Add(new Claim("ModeratorOfAuction", auction.AuctionId.ToString()));
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
