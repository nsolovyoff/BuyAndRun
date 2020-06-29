using BLL.Resources.Auction;

namespace BLL.Resources.Category
{
    public class UpdateCategoryResource
    {
        public string Title { get; set; }
        public AuctionResource ParentAuction { get; set; }
    }
}
