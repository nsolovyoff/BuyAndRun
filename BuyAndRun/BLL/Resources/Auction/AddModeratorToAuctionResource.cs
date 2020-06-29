using BLL.Resources.Identity.User;

namespace BLL.Resources.Auction
{
    public class AddModeratorToAuctionResource
    {
        public UserResource User { get; set; }
        public AuctionResource Auction { get; set; }
    }
}
