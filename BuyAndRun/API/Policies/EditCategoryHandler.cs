using System.Security.Claims;
using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Policies
{
    public class EditCategoryHandler : AuthorizationHandler<EditCategoryRequirement, int>
    {
        private readonly ICategoryService _categoryService;

        public EditCategoryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditCategoryRequirement requirement, int categoryId)
        {
            var category = await _categoryService.GetCategoryById(categoryId);
            var auctionId = category.ParentAuction.Id;
            var canModerate = ModeratorOfAuctionHelper.CanModerateAuction(context.User, auctionId);
            if (canModerate)
            {
                context.Succeed(requirement);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isOwner = category.StartedBy.Id == userId;
            if (isOwner)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }

    public class EditCategoryRequirement : IAuthorizationRequirement { }
}
