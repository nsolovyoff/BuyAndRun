using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Identity;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Policies
{
    public class EditLotHandler : AuthorizationHandler<EditLotRequirement, int>
    {
        private readonly ILotService _lotService;
        private readonly ICategoryService _categoryService;
        private readonly AuctionUserManager _userService;

        public EditLotHandler(ILotService lotService, AuctionUserManager userService, ICategoryService categoryService)
        {
            _lotService = lotService;
            _userService = userService;
            _categoryService = categoryService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditLotRequirement requirement, int lotId)
        {
            var p = await _lotService.GetLotById(lotId);
            var category = await _categoryService.GetCategoryById(p.CategoryId);
            var auctionId = category.ParentAuction.Id;
            var canModerate = ModeratorOfAuctionHelper.CanModerateAuction(context.User, auctionId);
            if (canModerate)
            {
                context.Succeed(requirement);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.FindByIdAsync(userId);
            var lots = user.Lots.Where(p => p.LotId == lotId).Count();
            if (lots > 0)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }

    public class EditLotRequirement : IAuthorizationRequirement { }
}
