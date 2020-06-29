using System.Linq;
using System.Security.Claims;

namespace API.Policies
{
    public static class ModeratorOfAuctionHelper
    {
        public static bool CanModerateAuction(ClaimsPrincipal User, int auctionId)
        {
            var claims = User.Claims.Where(c => c.Type == "ModeratorOfAuction" && c.Value == auctionId.ToString()).Count();
            return (User.IsInRole("Admins") || User.IsInRole("GlobalModerators") || claims > 0);
        }
    }
}