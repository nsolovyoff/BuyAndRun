using BLL.Resources.Auction;
using BLL.Resources.Identity.User;

namespace BLL.Resources.Category
{
    public class CreateCategoryResource
    {
        public string Title { get; set; }
        public AuctionResource ParentAuction { get; set; }
        public UserResource StartedBy { get; set; }
    }
}
