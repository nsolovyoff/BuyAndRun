using System;
using BLL.Resources.Auction;
using BLL.Resources.Identity.User;
using BLL.Resources.Lot;

namespace BLL.Resources.Category
{
    public class CategoryResource
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int LotsCount { get; set; }

        public AuctionResource ParentAuction { get; set; }
        public UserResource StartedBy { get; set; }
    }
}
