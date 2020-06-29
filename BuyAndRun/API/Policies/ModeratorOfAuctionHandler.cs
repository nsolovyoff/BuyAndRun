using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Policies
{
    public class ModeratorOfAuctionHandler : AuthorizationHandler<ModeratorOfAuctionRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModeratorOfAuctionRequirement requirement, int auctionId)
        {
            if (ModeratorOfAuctionHelper.CanModerateAuction(context.User, auctionId)) context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    public class ModeratorOfAuctionRequirement : IAuthorizationRequirement { }
}
